using System;
using System.Collections.Generic;
using Core.Services;
using SRDebugger.Services;
using SRF.Service;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core.Debug
{
    public class DebugMenu : IInitializable, ITickable
    {
        private HotKeyHelper _helper;
        private IAdvertisingService _advertisingService;
        private CustomDebugMenu _customDebugMenu;

        [Inject]
        private void Construct(IAdvertisingService advertisingService, CustomDebugMenu customDebugMenu)
        {
            _customDebugMenu = customDebugMenu;
            _advertisingService = advertisingService;
        }
        
        void IInitializable.Initialize()
        {
            SRDebug.Init();
            var options = SRServiceManager.GetService<IOptionsService>();
            options.AddContainer(_customDebugMenu);
            SRDebug.Instance.PanelVisibilityChanged += PanelVisibilityChanged;

            InitHotKeyHelper();
        }

        void ITickable.Tick()
        {
            _helper.Tick();
        }

        private void InitHotKeyHelper()
        {
            _helper = new HotKeyHelper(new Dictionary<KeyCode, Action>
            {
                { KeyCode.Z, () =>
                {
                    _customDebugMenu.CompleteLevel();
                }},

                { KeyCode.X, () =>
                {
                    _customDebugMenu.ReloadLevel();
                }},
                
                { KeyCode.C, () =>
                {
                    _customDebugMenu.PrevLevel();
                }}
            });
        }

        private void PanelVisibilityChanged(bool isVisible)
        {
            Time.timeScale = isVisible ? 0 : 1;
            
            if (isVisible)
            {
                _advertisingService.HideBanner();
            }
            else
            {
                _advertisingService.ShowBanner();
            }
        }
    }
}