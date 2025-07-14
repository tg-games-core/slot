using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.UI
{
    public class ToggleButton : Button
    {
        [SerializeField]
        private Image _image;

        [SerializeField]
        private TextMeshProUGUI _text;

        [SerializeField]
        private Sprite _activeSprite;

        [SerializeField]
        private Sprite _disabledSprite;

        private Action _clickAction;
        private Func<bool> _stateProvider;
        private Func<string> _textProvider;

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            OnButtonClick();
        }

        public void Initialize(Action clickAction, Func<bool> stateProvider, Func<string> textProvider = null)
        {
            _clickAction = clickAction;
            _stateProvider = stateProvider;
            _textProvider = textProvider;
        }
        
        public void Refresh()
        {
            SetImageState(_stateProvider?.Invoke() ?? false);
            UpdateText();
        }

        private void SetImageState(bool isActive)
        {
            _image.sprite = isActive ? _activeSprite : _disabledSprite;
        }

        private void UpdateText()
        {
            if (_textProvider != null)
            {
                _text.text = _textProvider.Invoke();
            }
        }

        private void OnButtonClick()
        {
            _clickAction?.Invoke();
        }
    }
}