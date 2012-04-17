using Lidgren.Network;

namespace MinionServer.Server
{
    /// <summary>
    /// An interface for incoming network messages
    /// </summary>
    public interface INetworkIncomingMessage
    {
        /// <summary>
        /// Type of message 
        /// </summary>
        MessageType Type
        {
            get; 
        }

        /// <summary>
        /// Decode message
        /// </summary>
        /// <param name="netInMsg">Incoming message</param>
        void Decode(NetIncomingMessage netInMsg);
    }
}
