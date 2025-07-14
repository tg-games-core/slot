using System;
using System.Threading;

namespace Core
{
    public static class UniTaskUtil
    {
        public static CancellationToken RefreshToken(ref CancellationTokenSource tokenSource)
        {
            CancelToken(ref tokenSource);
            tokenSource = new CancellationTokenSource();
            return tokenSource.Token;
        }

        public static void CancelToken(ref CancellationTokenSource tokenSource)
        {
            try
            {
                tokenSource?.Cancel();
            }
            catch (ObjectDisposedException) { }
            finally
            {
                tokenSource?.Dispose();
                tokenSource = null;
            }
        }
    }
}