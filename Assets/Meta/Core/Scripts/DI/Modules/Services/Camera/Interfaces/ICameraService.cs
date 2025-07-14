using Core.Camera.Controllers;

namespace Core.Services.Interfaces
{
    public interface ICameraService
    {
        UnityEngine.Camera Camera { get; }

        void Register(CameraController cameraController);
        void ChangeState(int state);
    }
}