using System.Threading;
using Cysharp.Threading.Tasks;

namespace Project.Autoplay.Interfaces
{
    public interface ICommand
    {
        UniTask Execute(CancellationToken token);
    }
}