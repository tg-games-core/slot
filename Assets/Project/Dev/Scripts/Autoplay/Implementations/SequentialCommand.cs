using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Autoplay.Interfaces;

namespace Project.Autoplay.Implementations
{
    public class SequentialCommand : ICommand
    {
        private readonly List<ICommand> _commands;

        public SequentialCommand(IEnumerable<ICommand> commands)
        {
            _commands = commands.ToList();
        }

        async UniTask ICommand.Execute(CancellationToken token)
        {
            foreach (var command in _commands)
            {
                await command.Execute(token);
            }
        }
    }
}