using Cognigy.Utility;
using System.Threading;

namespace Cognigy
{
    public static class CustomWaitHandle
    {
        public static bool CancelableWaitOne(this WaitHandle handle, int millisecondsTimeout, CancellationToken cancellationToken)
        {
            int n = WaitHandle.WaitAny(new[] { handle, cancellationToken.WaitHandle }, millisecondsTimeout);
            switch (n)
            {
                case WaitHandle.WaitTimeout: //Timeout
                    return false;
                case 0:                     //Passed Waithandle gets signaled
                    return true;
                default:                    //Cancellation via token
                    throw new CognigyOperationCanceledException();
            }
        }
    }
}
