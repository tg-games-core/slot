using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Autoplay.Interfaces;

namespace Project.Autoplay
{
    public class CommandQueue
    {
        private readonly Queue<ICommand> _queue = new();
        private bool _isRunning;

        public void Enqueue(ICommand command)
        {
            _queue.Enqueue(command);
            if (!_isRunning)
                _ = ProcessQueueAsync();
        }

        private async UniTask ProcessQueueAsync()
        {
            _isRunning = true;

            while (_queue.Count > 0)
            {
                var command = _queue.Dequeue();
                await command.Execute();
            }

            _isRunning = false;
        }

        public void Clear()
        {
            _queue.Clear();
        }
    }
}