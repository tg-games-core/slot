using Core.Services.Interfaces;
using Core.UI;
using UnityEngine;
using VContainer;

namespace Core.Camera.Controllers
{
    public abstract class CameraController : MonoBehaviour
    {
        [field: SerializeField]
        public UnityEngine.Camera Camera
        {
            get; private set;
        }
        
        [SerializeField]
        private Animator _animator;

        private int _currentState;
        
        private ICameraService _cameraService;
        private UISystem _uiSystem;

        [Inject]
        private void Construct(ICameraService cameraService, UISystem uiSystem)
        {
            _cameraService = cameraService;
			_uiSystem = uiSystem;
        }

        protected virtual void Awake()
        {
			// var cameraData = Camera.GetUniversalAdditionalCameraData();
   //          cameraData.cameraStack.Add(_uiSystem.Camera);

            _cameraService.Register(this);
        }
        
        public void ChangeState(int state)
        {
            if (_currentState != state)
            {
                _animator.ResetTrigger(_currentState);
                _animator.SetTrigger(state);

                _currentState = state;
            }
        }
    }
}