using Core.Data;
using Core.UI.MVC.Interface;
using Cysharp.Threading.Tasks;
using VContainer;
#if FORCE_DEBUG
using UnityEngine;
#endif

namespace Core.UI.MVC
{
    public abstract class Controller<TModel, TView> : IController
        where TModel : IModel
        where TView : class, IView
    {
        protected readonly ReactiveSubscribersContainer _reactiveContainer = new();
        
        protected IUser _user;
        protected TView _view;
        protected TModel _model;
        protected UISystem _uiSystem;

        [Inject]
        private void Construct(IUser user, UISystem uiSystem)
        {
            _user = user;
            _uiSystem = uiSystem;
        }

        bool IController.IsPopup
        {
            get => _view.IsPopup;
        }

        void IController.Build(IView view, IModel model)
        {
            _view = (TView)view;
            _model = (TModel)model;
        }

        public virtual void Initialize()
        {
            _view.Preload();
        }

        async UniTask IController.Show()
        {
            Bind();
            
            await _view.Show();
            _view.Refresh();
        }

        public virtual void Bind()
        {
            _view.Showing += IView_Showing;
            _view.Shown += IView_Shown;
            _view.Hiding += IView_Hiding;
            _view.Hidden += IView_Hidden;
            
            _user.CurrencyChanged += IUser_CurrencyChanged;
        }

        public virtual void Dispose()
        {
            _view.Showing -= IView_Showing;
            _view.Shown -= IView_Shown;
            _view.Hiding -= IView_Hiding;
            _view.Hidden -= IView_Hidden;
            
            _user.CurrencyChanged -= IUser_CurrencyChanged;
            
            _reactiveContainer.Dispose();
        }
        
        void IController.RestoreFocus()
        {
            OnFocusRestored();
        }

        void IController.LostFocus()
        {
            OnFocusLost();
        }

        async UniTask IController.Hide(bool isAnimationNeeded)
        {
            await _view.Hide(isAnimationNeeded);
            
            Dispose();
        }

        void IController.NavigateBack()
        {
            _uiSystem.NavigateBack();
        }

        void IController.BringToFront()
        {
            _view.Transform.SetAsLastSibling();
        }
        
        protected virtual void OnFocusRestored() { }
        protected virtual void OnFocusLost() { }
        protected virtual void OnShowing() { }
        protected virtual void OnShown() { }
        protected virtual void OnHiding() { }
        protected virtual void OnHide() { }
        
        protected T GetSharedData<T>(string key, T defaultValue = default)
        {
            if (_uiSystem.Data == null || _uiSystem.Data.Count == 0)
            {
                return defaultValue;
            }
            
            if (!_uiSystem.Data.TryGetValue(key, out object value))
            {
                return defaultValue;
            }

            return value is T typedValue ? typedValue : defaultValue;
        }

        protected void SetSharedData<T>(string key, T value)
        {
            _uiSystem.Data[key] = value;
        }
        
        private void IView_Showing()
        {
            OnShowing();
        }

        private void IView_Shown()
        {
            OnShown();
        }
        
        private void IView_Hiding()
        {
            OnHiding();
        }
        
        private void IView_Hidden()
        {
            OnHide();
        }

        private void IUser_CurrencyChanged(CurrencyType type, int delta)
        {
            _view.Refresh();
        }
        
#if FORCE_DEBUG
        void IController.ToggleUI(bool isVisible)
        {
            if (!_view.Transform.TryGetComponent(out CanvasGroup canvasGroup))
            {
                canvasGroup = _view.Transform.gameObject.AddComponent<CanvasGroup>();
            }
        
            canvasGroup.alpha = isVisible ? 1 : 0;
        }
#endif
    }
}