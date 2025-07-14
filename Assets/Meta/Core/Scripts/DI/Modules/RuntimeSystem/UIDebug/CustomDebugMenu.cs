using System.ComponentModel;
using Core.Data;
using Core.UI;
using UnityEngine;
using VContainer;

namespace Core.Debug
{
    public class CustomDebugMenu
    {
        private bool _isUIActive = true;
        
        private LevelFlowController _levelFlowController;
        private IProgressData _progressData;
        private IUser _user;

        [Inject]
        private void Construct(LevelFlowController levelFlowController, IProgressData progressData, IUser user)
        {
            _user = user;
            _progressData = progressData;
            _levelFlowController = levelFlowController;
        }

        [Category("Meta")]
        public void CompleteLevel()
        {
            _levelFlowController.Complete();
        }

        [Category("Meta")]
        public async void ReloadLevel()
        {
            await _levelFlowController.Load();
        }

        [Category("Meta")]
        public async void PrevLevel()
        {
            _progressData.SetLevelIndex(_progressData.LevelIndex.CurrentValue - 1);

            await _levelFlowController.Load();
        }
        
        [Category("Meta")]
        public async void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            
            await _levelFlowController.Load();
        }
        
#if FORCE_DEBUG
        [Category("UA")]
        public void ToggleUI()
        {
            _isUIActive = !_isUIActive;

            var uiSystem = Object.FindAnyObjectByType<UISystem>();
            uiSystem?.ToggleUI(_isUIActive);
        }
#endif
        
        [Category("Meta")]
        public void AddCoins()
        {
            _user.ClaimReward(new RewardItem(RewardType.Money, 10000));   
        }
    }
}