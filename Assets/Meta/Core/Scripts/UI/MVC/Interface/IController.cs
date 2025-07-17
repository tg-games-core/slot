using System;
using Cysharp.Threading.Tasks;

namespace Core.UI.MVC.Interface
{
    public interface IController : IDisposable
    {
        bool IsPopup { get; }
        
        void Build(IView view, IModel model);
        void Initialize();
        UniTask Show();
        void Bind();
        void RestoreFocus();
        void LostFocus();
        UniTask Hide(bool isAnimationNeeded);
        void NavigateBack();
        void BringToFront();
        
#if FORCE_DEBUG
        void ToggleUI(bool isVisible);
#endif
    }
}