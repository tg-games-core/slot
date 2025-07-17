using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Autoplay.Interfaces;

namespace Project.Autoplay.Implementations
{
    public class Command : ICommand
    {
        private readonly Action _action;

        public Command(Action action)
        {
            _action = action;
        }
        
        UniTask ICommand.Execute(CancellationToken token)
        {
            _action?.Invoke();
            
            return UniTask.CompletedTask;
        }
    }
}