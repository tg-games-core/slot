using Core.Services.Interfaces;
using UnityEngine;
using VContainer;

namespace Core.GamePlay.UI
{
    public class GameInterface : MonoBehaviour
    {
        [SerializeField]
        private Canvas _canvas;

        private ICameraService _cameraService;

        [Inject]
        private void Construct(ICameraService cameraService)
        {
            _cameraService = cameraService;
        }
        
        private void Awake() { }

        private void Start()
        {
            _canvas.renderMode = RenderMode.ScreenSpaceCamera;
            _canvas.worldCamera = _cameraService.Camera;
        }

        private void LateUpdate() { }
        
        private void Initialize<T>(ref T[] array, T prefab, int arraySize) where T : WorldToScreenElement
        {
            array = new T[arraySize];
            
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = Instantiate(prefab, transform);
                array[i].Initialize(this);
                array[i].Hide();
            }
        }

        public Vector2 GetViewportPosition(Vector3 worldPosition)
        {
            return _cameraService.Camera.WorldToViewportPoint(worldPosition);
        }
    }
}