using System;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Core
{
    public class SceneLoadingService
    {
        public event Action LoadingStarted;
        public event Action Loaded;

        public async UniTask Load(string name, Action callback = null)
        {
            LoadingStarted?.Invoke();
            
            await SceneManager.LoadSceneAsync(name).ToUniTask();
            
            callback?.Invoke();
            
            Loaded?.Invoke();
        }

        public async UniTask Load(string name, Action<float> progress, Action callback = null)
        {
            LoadingStarted?.Invoke();
            var loadingOperation = SceneManager.LoadSceneAsync(name);

            if (loadingOperation != null)
            {
                await UniTask.WaitWhile(() =>
                {
                    progress?.Invoke(loadingOperation.progress);
                    return !loadingOperation.isDone;
                });
            }
            
            progress?.Invoke(1f);
            callback?.Invoke();
            
            Loaded?.Invoke();
        }
    }
}