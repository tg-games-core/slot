using System;
using Cysharp.Threading.Tasks;
using Project.Autoplay.Interfaces;

namespace Project.Autoplay.Implementations
{
    public class ConditionalCommand : ICommand
    {
        private readonly Func<bool> _condition;
        private readonly ICommand _innerCommand;

        public ConditionalCommand(Func<bool> condition, ICommand innerCommand)
        {
            _condition = condition;
            _innerCommand = innerCommand;
        }

        async UniTask ICommand.Execute()
        {
            if (_condition())
            {
                await _innerCommand.Execute();
            }
        }
    }
}