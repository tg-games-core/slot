using Core.Services;
using Core.UI;
using Project.Autoplay.Implementations;
using Project.Autoplay.Interfaces;
using UnityEngine;
using VContainer;

namespace Project.Autoplay
{
    public class AutoplayService : IAutoplayService
    {
        private readonly CommandQueue _commandQueue = new();
        
        private IRuntimeRegistry _runtimeRegistry;
        private UISystem _uiSystem;

        [Inject]
        private void Construct(IRuntimeRegistry runtimeRegistry, UISystem uiSystem)
        {
            _runtimeRegistry = runtimeRegistry;
            _uiSystem = uiSystem;
        }

        void IAutoplayService.StartAutoplay()
        {
            var command = new Command(() => { Debug.Log("Spend Money!"); });
            var uiCommand = new UICommand<ResultWindow>(_uiSystem, command, null);
            _commandQueue.Enqueue(uiCommand);
            _commandQueue.Run();
        }
    }
}