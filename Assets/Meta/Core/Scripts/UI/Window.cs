using System;
using Core.UI.MVC;
using Core.UI.MVC.Interface;
using Core.Wrappers.Animations;
using Cysharp.Threading.Tasks;
using UnityEngine;
#if FireTV
using UnityEngine.EventSystems;
#endif
using UnityEngine.UI;
using VContainer;

namespace Core.UI
{
    public abstract class Window<TModel, TController> : MonoBehaviour, IView, IViewInitializer
        where TModel : Model, new()
        where TController : IController, new()
    {
        public event Action Showing;
        public event Action Shown;
        public event Action Hiding;
        public event Action Hidden;

#if FireTV
        [SerializeField]
        private GameObject _selectedObject;
#endif

        [SerializeField]
        private Button _backButton;

        [SerializeField]
        private AnimationWrapper _showAnimation;

        [SerializeField]
        private AnimationWrapper _hideAnimation;

        protected TController _controller;
        private IObjectResolver _objectResolver;

        public abstract bool IsPopup { get; }
        public Transform Transform { get => transform; }
        
        [Inject]
        private void Construct(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }

        public virtual void Enable() { }
        public virtual void Disable() { }

        protected virtual void Start()
        {
            if (_backButton != null)
            {
                _backButton.onClick.AddListener(OnBackButtonClicked);
            }
        }

        protected void OnDestroy()
        {
            _controller?.Dispose();
        }

        public virtual void Preload()
        {
            gameObject.SetActive(false);
        }

        IController IViewInitializer.Initialize()
        {
            _controller = _objectResolver.ResolvePresenter<TController, TModel>(this);

            return _controller;
        }

        async UniTask IView.Show()
        {
            gameObject.SetActive(true);
            Enable();
            
            Showing?.Invoke();
            
#if FireTV
            SetDefaultSelection();
#endif

            if (_showAnimation != null)
            {
                _showAnimation.Play();
                await UniTask.Delay(TimeSpan.FromSeconds(_showAnimation.Duration));
            }
            
            Shown?.Invoke();
        }

        async UniTask IView.Hide(bool isAnimationNeeded)
        {
            Hiding?.Invoke();
            
            await ExecuteHide(isAnimationNeeded);
            Disable();
            
            Hidden?.Invoke();
        }

#if FireTV
        public virtual void SetDefaultSelection()
        {
            if (_selectedObject != null)
            {
                EventSystem.current.SetSelectedGameObject(_selectedObject);
            }
            else
            {
                DebugSafe.LogError($"Default selected object is not set for {name}");
            }
        }
#endif

        public virtual void Refresh() { }

        protected virtual async UniTask ExecuteHide(bool isAnimationNeeded)
        {
            if (isAnimationNeeded && _hideAnimation != null)
            {
                _hideAnimation.Play();
                await UniTask.Delay(TimeSpan.FromSeconds(_hideAnimation.Duration), DelayType.Realtime);
            }
            
#if FireTV
            EventSystem.current.SetSelectedGameObject(null);
#endif

            gameObject.SetActive(false);
        }

        protected virtual void OnBackButtonClicked()
        {
            _controller.NavigateBack();
        }
    }
}