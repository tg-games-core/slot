using System;
using Cysharp.Threading.Tasks;
using Project.Autoplay.Interfaces;

namespace Project.Autoplay.Implementations
{
    public class Command : ICommand
    {
        private readonly Action _command;

        public Command(Action command)
        {
            _command = command;
        }
        
        UniTask ICommand.Execute()
        {
            _command?.Invoke();
            
            return UniTask.CompletedTask;
        }
    }
}