using System;

namespace Core
{
    public interface ICommand
    {
        event Action<ICommand, bool> Completed;
        
        void Execute();
    }
}