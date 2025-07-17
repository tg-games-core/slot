using System.Collections.Generic;
using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using ICommand = Project.Autoplay.Interfaces.ICommand;

namespace Project.Autoplay
{
    public class CommandQueue
    {
        private readonly Queue<ICommand> _queue = new();
        
        private bool _isRunning;

        private CancellationTokenSource _token;

        public void Enqueue(ICommand command)
        {
            _queue.Enqueue(command);
        }

        public void Run()
        {
            if (!_isRunning)
            {
                _ = ProcessQueueAsync(UniTaskUtil.RefreshToken(ref _token));
            }
        }

        private async UniTask ProcessQueueAsync(CancellationToken token)
        {
            _isRunning = true;

            while (_queue.Count > 0)
            {
                var command = _queue.Dequeue();
                await command.Execute(token);
            }

            _isRunning = false;
        }

        public void Clear()
        {
            _queue.Clear();
        }
    }
}