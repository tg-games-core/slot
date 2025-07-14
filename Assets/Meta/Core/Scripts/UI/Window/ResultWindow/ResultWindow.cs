using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class ResultWindow : Window<ResultModel, ResultController>
    {
        public event Action OnContinueClicked;
        
        [SerializeField]
        private Button _continueButton;

        public override bool IsPopup
        {
            get => false;
        }

        protected override void Start()
        {
            base.Start();
            
            _continueButton.onClick.AddListener(OnContinueButtonClicked);
        }

        private void OnContinueButtonClicked()
        {
            OnContinueClicked?.Invoke();
        }
    }
}