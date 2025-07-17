using System.Collections.Generic;
using System.Linq;
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

        async UniTask ICommand.Execute()
        {
            var tasks = _commands.Select(cmd => cmd.Execute());
            
            await UniTask.WhenAll(tasks);
        }
    }
}