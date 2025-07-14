using Core.Camera.Controllers;
using Core.Services.Interfaces;

namespace Core.Services.Implementations
{
    public class CameraService : ICameraService
    {
        private CameraController _cameraController;

        public UnityEngine.Camera Camera
        {
            get => _cameraController?.Camera;
        }

        public void Register(CameraController cameraController)
        {
            _cameraController = cameraController;
        }

        public void ChangeState(int state)
        {
            _cameraController?.ChangeState(state);
        }
    }
}