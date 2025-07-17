using Core.Services;
using Project.Autoplay.Implementations;
using Project.Autoplay.Interfaces;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Project.Autoplay
{
    public class AutoplayService : IAutoplayService, IInitializable
    {
        private CommandQueue _commandQueue;
        private IRuntimeRegistry _runtimeRegistry;

        [Inject]
        private void Construct(IRuntimeRegistry runtimeRegistry)
        {
            _runtimeRegistry = runtimeRegistry;
        }
        
        void IInitializable.Initialize()
        {
            _commandQueue = new CommandQueue();
        }

        void IAutoplayService.StartAutoplay()
        {
            _commandQueue.Enqueue(new Command(() => { Debug.Log("Spend Money!");}));
        }
    }
}