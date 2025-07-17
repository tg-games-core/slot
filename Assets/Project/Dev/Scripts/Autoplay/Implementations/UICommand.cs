using System;
using System.Collections.Generic;
using Core.UI;
using Core.UI.MVC.Interface;
using Cysharp.Threading.Tasks;
using Project.Autoplay.Interfaces;

namespace Project.Autoplay.Implementations
{
    public class UICommand<T> : ICommand where T : IView
    {
        private readonly UISystem _uiSystem;
        private readonly Func<Dictionary<string, object>> _dataFunc;

        public UICommand(UISystem uiSystem, Func<Dictionary<string, object>> dataFunc)
        {
            _uiSystem = uiSystem;
            _dataFunc = dataFunc;
        }
        
        async UniTask ICommand.Execute()
        {
            _uiSystem.ShowWindow<T>(_dataFunc?.Invoke());

            await _uiSystem.TransitionTask;
            await _uiSystem.NavigateBack();
        }
    }
}