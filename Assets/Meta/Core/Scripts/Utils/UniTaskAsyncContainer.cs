using System;
using System.Collections.Generic;
using System.Threading;

namespace Core
{
    public class UniTaskAsyncContainer : IDisposable
    {
        private readonly List<CancellationTokenSource> _asyncOperations = new();

        public CancellationToken RefreshToken(ref CancellationTokenSource tokenSource)
        {
            _asyncOperations.Remove(tokenSource);
            var token = UniTaskUtil.RefreshToken(ref tokenSource);
            _asyncOperations.Add(tokenSource);
            return token;
        }

        public void CancelToken(ref CancellationTokenSource tokenSource)
        {
            if (_asyncOperations.Remove(tokenSource))
            {
                UniTaskUtil.CancelToken(ref tokenSource);
            }
        }
        
        public void Dispose()
        {
            var sourcesToCancel = _asyncOperations.ToArray();
        
            foreach (var source in sourcesToCancel)
            {
                var tempSource = source;
                CancelToken(ref tempSource);
            }
            
            _asyncOperations.Clear();
        }
    }
}