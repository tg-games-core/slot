using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.UI.MVC.Interface
{
    public interface IView
    {
        public event Action Showing;
        public event Action Shown;
        public event Action Hiding;
        public event Action Hidden;
        
        bool IsPopup { get; }
        Transform Transform { get; }

        void Preload();
        void Enable();
        void Disable();
        UniTaskVoid Show();
        void Refresh();
        UniTask Hide(bool isAnimationNeeded);
    }
}