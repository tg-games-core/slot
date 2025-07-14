using System;
using System.Collections.Generic;
using Core.Highlighting.Interfaces;
using UnityEngine;

namespace Core.Highlighting
{
    public class HighlightComposer : IDisposable
    {
        private readonly MaterialPropertyBlock _materialPropertyBlock;
        private readonly Action<MaterialPropertyBlock> _onPropertiesUpdated;
        private readonly List<IHighlightEffect> _effects = new();

        public HighlightComposer(MaterialPropertyBlock materialPropertyBlock,
            Action<MaterialPropertyBlock> onPropertiesUpdated)
        {
            _materialPropertyBlock = materialPropertyBlock;
            _onPropertiesUpdated = onPropertiesUpdated;
        }

        public void AddEffect(IHighlightEffect highlightEffect)
        {
            _effects.Add(highlightEffect);
        }

        public void ResetAllEffects()
        {
            for (int i = 0; i < _effects.Count; i++)
            {
                _effects[i].Reset(_materialPropertyBlock);
            }
            
            _onPropertiesUpdated?.Invoke(_materialPropertyBlock);
        }

        public void ApplyEffects(float progress)
        {
            for (int i = 0; i < _effects.Count; i++)
            {
                _effects[i].Apply(_materialPropertyBlock, progress);
            }
            
            _onPropertiesUpdated?.Invoke(_materialPropertyBlock);
        }

        public void Dispose()
        {
            _effects.Clear();
        }
    }
}