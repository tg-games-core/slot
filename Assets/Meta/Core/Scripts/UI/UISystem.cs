using System;
using System.Collections.Generic;
using Core.UI.MVC.Interface;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class UISystem : MonoBehaviour
    {
        private readonly Stack<IController> _controllerStack = new();
        private readonly Stack<IController> _pendingControllers = new();
        private readonly Dictionary<string, object> _sharedData = new();
        private readonly Dictionary<IView, IController> _viewControllerMap = new();

        [SerializeField]
        private GameObject _windowsContainer;

        [SerializeField]
        private UnityEngine.Camera _camera;

        private bool _isViewTransitionInProgress;

        private IController _currentController;

        private bool IsSingleView
        {
            get => _controllerStack.Count <= 1;
        }
        
        public UnityEngine.Camera Camera
        {
            get => _camera;
        }

        public Dictionary<string, object> Data
        {
            get => _sharedData;
        }

        private void Awake()
        {
            InitializeViews();
            ConfigureCanvas();
            DontDestroyOnLoad(gameObject);
        }
        
        private void InitializeViews()
        {
            foreach (var view in _windowsContainer.GetComponentsInChildren<IView>(true))
            {
                var controller = ((IViewInitializer)view).Initialize();
                _viewControllerMap.Add(view, controller);
                controller.Initialize();
            }
        }
        
        private void ConfigureCanvas()
        {
            if (TryGetComponent(out CanvasScaler canvasScaler))
            {
                canvasScaler.matchWidthOrHeight = UIUtils.GetMatchWidthOrHeight();
            }
        }
        
        public void ShowWindow<T>(Dictionary<string, object> data = null) where T : IView
        {
            MergeData(data);

            var controller = GetController<T>();
            if (_currentController == controller)
            {
                return;
            }

            _pendingControllers.Push(controller);

            if (!_isViewTransitionInProgress)
            {
                ProcessViewTransition().Forget();
            }
        }

        public IView GetView<T>() where T : IView
        {
            foreach (var map in _viewControllerMap)
            {
                if (map.Key is T)
                {
                    return map.Key;
                }
            }

            return null;
        }
        
        private async UniTaskVoid ProcessViewTransition()
        {
            while (_pendingControllers.Count > 0)
            {
                var controller = _pendingControllers.Pop();
                if (_currentController == controller)
                {
                    return;
                }

                _isViewTransitionInProgress = true;

                if (controller.IsPopup)
                {
                    controller.BringToFront();
                }
                else
                {
                    await HideViewsUntilReaching(controller);
                }

                _controllerStack.Push(controller);
				_currentController?.LostFocus();
                _currentController = controller;
                await controller.Show();
                _isViewTransitionInProgress = false;
            }
        }

        private async UniTask HideViewsUntilReaching(IController targetController)
        {
            while (_controllerStack.Count > 0)
            {
                var top = _controllerStack.Peek();
                if (top == targetController)
                {
                    break;
                }

                await _controllerStack.Pop().Hide(IsSingleView);
            }
        }
        
        public async UniTaskVoid NavigateBack()
        {
            if (_controllerStack.Count > 0)
            {
                await _controllerStack.Pop().Hide(true);
            }

            if (_controllerStack.Count == 0)
            {
                ShowWindow<GameWindow>();
            }
                
            _currentController = _controllerStack.Peek();
            _currentController.RestoreFocus();
        }

        private IController GetController<T>() where T : IView
        {
            foreach (var view in _viewControllerMap)
            {
                if (view.Key is T)
                {
                    return view.Value;
                }
            }

            DebugSafe.LogException(new Exception($"View of type {typeof(T)} not found!"));
            return null;
        }

        private void MergeData(Dictionary<string, object> newData)
        {
            if (newData == null)
                return;
            
            foreach (var record in newData)
            {
                _sharedData[record.Key] = record.Value;
            }
        }
        
#if FORCE_DEBUG
        public void ToggleUI(bool isVisible)
        {
            foreach (var controller in _viewControllerMap.Values)
            {
                controller.ToggleUI(isVisible);
            }
        }
#endif
    }
}