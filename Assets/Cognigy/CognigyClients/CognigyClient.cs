using Quobject.SocketIoClientDotNet.Client;
using System.Threading;

namespace Cognigy
{
    public abstract class CognigyClient
    {
        public abstract void Connect(CancellationToken cxToken, int millisecondsTimeout);

        public bool Initialized { get; protected set; }
        public bool Cancelable { get; protected set; }

        public bool IsConnected()
        {
            return isConnected;
        }

        public void Disconnect()
        {
            if (this.mySocket != null && this.isConnected)
            {
                Initialized = false;
                this.isConnected = false;
                this.mySocket.Disconnect();
                this.mySocket.Close();
            }
        }

        protected Socket mySocket;
        protected bool isConnected;

        protected static AutoResetEvent waitHandle = new AutoResetEvent(false);
        protected CancellationToken cxToken = CancellationToken.None;
        protected int millisecondsTimeout = Timeout.Infinite;
    }
}
