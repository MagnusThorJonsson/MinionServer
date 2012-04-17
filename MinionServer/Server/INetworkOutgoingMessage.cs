using Lidgren.Network;

namespace MinionServer.Server
{
    /// <summary>
    /// An interface for outgoing network messages
    /// </summary>
    public interface INetworkOutgoingMessage
    {
        /// <summary>
        /// Type of message 
        /// </summary>
        MessageType Type
        {
            get;
        }

        /// <summary>
        /// Encode message
        /// </summary>
        void Encode(NetOutgoingMessage netOutMsg);

        void Send(NetClient client);
    }
}
