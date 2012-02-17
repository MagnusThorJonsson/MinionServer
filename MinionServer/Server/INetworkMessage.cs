using MinionServer.Server;

namespace MinionServer.Server
{
    interface INetworkMessage
    {
        MessageType Type
        { 
            get;
            set; 
        }

        void Encode();

        void Decode();
    }
}
