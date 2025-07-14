using System.Collections.Generic;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Bootstrap.States.Assets
{
    public interface IAssetBootstrapper
    {
        UniTask Initialize();
        IEnumerable<IRuntimeSystem> GetLoadedSystems();
        IEnumerable<ScriptableObject> GetLoadedSettings();
        T GetSystem<T>() where T : IRuntimeSystem;
        bool TryGetSystem<T>(out T manager) where T : IRuntimeSystem;
    }
}