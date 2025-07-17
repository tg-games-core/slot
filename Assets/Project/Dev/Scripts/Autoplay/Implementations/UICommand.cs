using System;
using System.Collections.Generic;
using System.Threading;
using Core.UI;
using Core.UI.MVC.Interface;
using Cysharp.Threading.Tasks;
using Project.Autoplay.Interfaces;
using UnityEngine;

namespace Project.Autoplay.Implementations
{
    public class UICommand<T> : ICommand, IDisposable where T : IView
    {
        private readonly UISystem _uiSystem;
        private readonly ICommand _command;
        private readonly Func<Dictionary<string, object>> _dataFunc;

        private bool _isShown;
        private bool _isHidden;
        
        private IView _view;

        public UICommand(UISystem uiSystem, ICommand command, Func<Dictionary<string, object>> dataFunc)
        {
            _uiSystem = uiSystem;
            _command = command;
            _dataFunc = dataFunc;
        }
        
        async UniTask ICommand.Execute(CancellationToken token)
        {
            InitializeViewListeners(_uiSystem.GetView<T>());
            _uiSystem.ShowWindow<T>(_dataFunc?.Invoke());

            await UniTask.WaitUntil(IsViewNotShown(), cancellationToken: token);

            if (_command != null)
            {
                await _command.Execute(token);
            }
            
            _uiSystem.NavigateBack().Forget();
            
            await UniTask.WaitUntil(IsViewNotHidden(), cancellationToken: token);
            
            ((IDisposable)this).Dispose();
        }

        void IDisposable.Dispose()
        {
            _view.Shown -= IView_Shown;
            _view.Hidden -= IView_Hidden;
        }

        private Func<bool> IsViewNotShown()
        {
            return () => _isShown;
        }
        
        private Func<bool> IsViewNotHidden()
        {
            return () => _isHidden;
        }

        private void InitializeViewListeners(IView view)
        {
            _view = view;
            
            _view.Shown += IView_Shown;
            _view.Hidden += IView_Hidden;
        }

        private void IView_Shown()
        {
            _isShown = true;
        }

        private void IView_Hidden()
        {
            _isHidden = true;
        }
    }
}