using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Autoplay.Interfaces;

namespace Project.Autoplay.Implementations
{
    public class ParallelCommand : ICommand
    {
        private readonly List<ICommand> _commands;

        public ParallelCommand(IEnumerable<ICommand> commands)
        {
            _commands = commands.ToList();
        }

        async UniTask ICommand.Execute(CancellationToken token)
        {
            var tasks = _commands.Select(command => command.Execute(token));
            
            await UniTask.WhenAll(tasks);
        }
    }
}