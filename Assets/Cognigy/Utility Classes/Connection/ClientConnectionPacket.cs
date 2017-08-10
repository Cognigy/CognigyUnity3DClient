using System.Threading;

namespace Cognigy
{
    public class ClientConnectionPacket
    {
        public CognigyClient Client { get; private set; }
        public CancellationToken Token { get; private set; }
        public int MillisecondsTimeout { get; private set; }

        public ClientConnectionPacket(CognigyClient client, CancellationToken token, int millisecondsTimeout)
        {
            this.Client = client;
            this.Token = token;
            this.MillisecondsTimeout = millisecondsTimeout;
        }
    }
}
