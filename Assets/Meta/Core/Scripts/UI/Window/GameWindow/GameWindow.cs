using Project.Plinko.Types;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class GameWindow : Window<GameModel, GameController>
    {
        [SerializeField]
        private Button _startButton;

        [SerializeField]
        private Button _cashoutButton;

        [SerializeField]
        private TextMeshProUGUI _balanceLabel;
        
        [SerializeField]
        private TextMeshProUGUI _multiplierLabel;

        [SerializeField]
        private TextMeshProUGUI _potentialWinLabel;

        [SerializeField]
        private TextMeshProUGUI _transactionLabel;

        [SerializeField]
        private TextMeshProUGUI _gameStatusLabel;
        
        public override bool IsPopup
        {
            get => false;
        }

        protected override void Start()
        {
            base.Start();
            
            _startButton.onClick.AddListener(OnStartButtonClicked);
            _cashoutButton.onClick.AddListener(OnCashoutButtonClicked);
        }
        
        private void OnStartButtonClicked()
        {
            _controller.Start();
        }
        
        private void OnCashoutButtonClicked()
        {
            _controller.Cashout();
        }

        public void RefreshButtons(PlinkoStateType plinkoStateType, float multiplier)
        {
            bool canShowCashout = plinkoStateType == PlinkoStateType.WaitingForCashout && multiplier >= 2.0f;
            
            _startButton.gameObject.SetActive(plinkoStateType != PlinkoStateType.WaitingForCashout);
            _cashoutButton.gameObject.SetActive(canShowCashout);
        }

        public void RefreshMultiplier(float multiplier)
        {
            _multiplierLabel.text = $"Мультипликатор: x{multiplier:F2}";
        }

        public void RefreshPotentialWin(float potentialWin)
        {
            _potentialWinLabel.text = $"Потенциальный выигрыш: ${potentialWin:F2}";
        }

        public void RefreshBalance(float balance)
        {
            _balanceLabel.text = $"Баланс: ${balance:F2}";
        }

        public void ShowTransaction(float delta, Color color)
        {
            var sign = delta > 0 ? '+' : '-';
            _transactionLabel.text = $"{sign}${Mathf.Abs(delta):F2}";
            _transactionLabel.color = color;
            _transactionLabel.gameObject.SetActive(true);
        }

        public void SetupGameStatus(string message)
        {
            _gameStatusLabel.text = message;
        }
        
        public void HideTransaction()
        {
            _transactionLabel.gameObject.SetActive(false);
        }
    }
}