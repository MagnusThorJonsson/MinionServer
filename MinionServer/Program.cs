using System;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Lidgren.Network;

using Minion2D.Helpers;

using MinionServer.Server;
using MinionServer.Game.Entities;
using MinionServer.Game.Map;

namespace MinionServer
{
    class Program
    {
        #region Map Constants
        private const int MAP_WIDTH = 100;
        private const int MAP_HEIGHT = 100;
        private const int TILE_WIDTH = 8;
        private const int TILE_HEIGHT = 8;
        #endregion

        private static TileMap _tileMap;
        private static Player[] _players;
        private static byte _playerCount;

        private static NetServer server;

        static void Main(string[] args)
        {
            // Initialization process
            Initialize();
            LoadMap();

            // schedule initial sending of position updates
            double nextSendUpdates = NetTime.Now;

            // run until escape is pressed
            NetIncomingMessage msg;
            while (!Console.KeyAvailable || Console.ReadKey().Key != ConsoleKey.Escape)
            {
                while ((msg = server.ReadMessage()) != null)
                {
                    // Handle incoming messages
                    HandleIncoming(msg);

                    // send position updates 30 times per second
                    if (NetTime.Now > nextSendUpdates)
                    {
                        // Handle player collisions
                        //HandleCollisions(NetTime.Now - nextSendUpdates);

                        // Handle outgoing messages
                        HandleOutgoing();

                        // schedule next update
                        nextSendUpdates += (1.0 / 60.0);
                    }
                }
                // sleep to allow other processes to run smoothly
                Thread.Sleep(1);
            }

            server.Shutdown("Server shutting down");
        }

        static void Initialize()
        {
            // Initialize
            _players = new Player[8];
            _playerCount = 0;

            NetPeerConfiguration config = new NetPeerConfiguration("xnaapp");
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.Port = 14242;

            // create and start server
            server = new NetServer(config);
            server.Start();
        }


        static void LoadMap()
        {
            // Set the tileMap array to the map size
            _tileMap = new TileMap(
                new Point(MAP_WIDTH, MAP_HEIGHT),
                new Point(TILE_WIDTH, TILE_HEIGHT)
            );
            _tileMap.GenerateMap();
        }

        static void HandleIncoming(NetIncomingMessage msg)
        {
            switch (msg.MessageType)
            {
                case NetIncomingMessageType.DiscoveryRequest:
                    // Server received a discovery request from a client; send a discovery response (with no extra data attached)
                    server.SendDiscoveryResponse(null, msg.SenderEndpoint);
                    break;

                // ERRORS
                case NetIncomingMessageType.VerboseDebugMessage:
                case NetIncomingMessageType.DebugMessage:
                case NetIncomingMessageType.WarningMessage:
                case NetIncomingMessageType.ErrorMessage:
                    //
                    // Just print diagnostic messages to console
                    //
                    Console.WriteLine(msg.ReadString());
                    break;

                case NetIncomingMessageType.StatusChanged:
                    NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();
                    if (status == NetConnectionStatus.Connected)
                    {
                        //
                        // A new player just connected!
                        //
                        Console.WriteLine(NetUtility.ToHexString(msg.SenderConnection.RemoteUniqueIdentifier) + " connected!");
                        /*
                        // randomize his position and store in connection tag
                        msg.SenderConnection.Tag = new int[] {
                            NetRandom.Instance.Next(10, 100),
                            NetRandom.Instance.Next(10, 100)
                        };
                        */

                        // TODO: Fix the way we tag players with an id, this does not handle disconnects
                        //_playerCount++;
                        //Console.WriteLine("Player Count: " + _playerCount);
                        //_players[_playerCount-1] = ;
                        msg.SenderConnection.Tag = new Player(1, 48, 64);

                        // Send an id to the player
                        NetOutgoingMessage response = server.CreateMessage();
                        response.Write((byte)MessageType.ConnectionAccepted);
                        response.Write((byte)1); // ID
                        response.Write(32); // Pos X
                        response.Write(32); // Pos Y
                        response.Write("Magic Server!"); // Server name
                        server.SendMessage(response, msg.SenderConnection, NetDeliveryMethod.ReliableOrdered);

                        /*
                        // Let everybody know about the new player
                        foreach (NetConnection player in server.Connections)
                        {
                            if (((Player)player.Tag).NetworkId != _playerCount)
                            {
                                response = server.CreateMessage();
                                response.Write((byte)ActionType.PlayerConnected);
                                response.Write(_playerCount);
                                server.SendMessage(response, player, NetDeliveryMethod.ReliableOrdered);
                            }
                        }
                        */
                    }
                    break;

                case NetIncomingMessageType.Data:
                    // The client sent input to the server
                    int xinput = msg.ReadInt32();
                    int yinput = msg.ReadInt32();

                    Player currPlayer = msg.SenderConnection.Tag as Player;

                    // fancy movement logic goes here; we just append input to position
                    currPlayer.PosX += xinput;
                    currPlayer.PosY += yinput;
                    break;
            }
            // Recycle to avoid garbage
            server.Recycle(msg);
        }

        static void HandleOutgoing()
        {
            Player currPlayer = null;
            // Yes, it's time to send position updates
            // for each player...
            foreach (NetConnection player in server.Connections)
            {
                // ... send information about every other player (actually including self)
                foreach (NetConnection otherPlayer in server.Connections)
                {
                    // send position update about 'otherPlayer' to 'player'
                    NetOutgoingMessage om = server.CreateMessage();

                    // write who this position is for
                    if (otherPlayer.Tag != null)
                    {
                        currPlayer = otherPlayer.Tag as Player;
                        om.Write((byte)MessageType.PlayerUpdate);
                        om.Write(currPlayer.NetworkId);
                        om.Write(currPlayer.PosX);
                        om.Write(currPlayer.PosY);

                        // send message
                        server.SendMessage(om, player, NetDeliveryMethod.Unreliable);
                    }

                    //if (otherPlayer.Tag == null)
                    //    otherPlayer.Tag = new int[2];
                }
            }
        }

    }
}