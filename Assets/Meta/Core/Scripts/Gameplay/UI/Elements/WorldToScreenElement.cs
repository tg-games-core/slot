using Core.UI;
using UnityEngine;

namespace Core.GamePlay.UI
{
    public abstract class WorldToScreenElement : MonoBehaviour
    {
        protected readonly ReactiveSubscribersContainer _reactiveContainer = new();
        
        [SerializeField]
        protected Vector3 _offset;

        [SerializeField]
        protected float _positionLerpSpeed;

        private Vector2 _targetScreenPosition;

        private RectTransform _transform;
        private GameInterface _gameInterface;

        public bool IsActive
        {
            get => gameObject.activeSelf;
        }

        protected abstract Vector3 WorldPosition { get; }

        protected virtual void Awake()
        {
            _transform = (RectTransform)transform;
        }

        protected virtual void OnEnable() { }

        protected virtual void OnDisable()
        {
            _reactiveContainer.Dispose();
        }

        public void Initialize(GameInterface gameInterface)
        {
            _gameInterface = gameInterface;
        }

        protected void Show()
        {
            gameObject.SetActive(true);
            UpdatePositionImmediate();
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        public virtual void Refresh()
        {
            if (!IsActive)
            {
                return;
            }

            UpdateTargetPosition();
            ApplySmoothedPosition();
        }
        
        private void UpdatePositionImmediate()
        {
            _targetScreenPosition = GetViewportPosition();
            _transform.SetViewportPosition(_targetScreenPosition);
        }
        
        private void UpdateTargetPosition()
        {
            _targetScreenPosition = GetViewportPosition();
        }

        private void ApplySmoothedPosition()
        {
            var currentPosition = _transform.anchoredPosition;
            var newPosition = Vector2.Lerp(currentPosition, _targetScreenPosition, _positionLerpSpeed * Time.deltaTime);
            
            _transform.SetViewportPosition(newPosition);
        }
        
        private Vector2 GetViewportPosition()
        {
            return _gameInterface.GetViewportPosition(WorldPosition + _offset);
        }
    }
}