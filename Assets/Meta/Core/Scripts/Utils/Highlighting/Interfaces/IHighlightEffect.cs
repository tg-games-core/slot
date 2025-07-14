using UnityEngine;

namespace Core.Highlighting.Interfaces
{
    public interface IHighlightEffect
    {
        void Reset(MaterialPropertyBlock materialPropertyBlock);
        void Apply(MaterialPropertyBlock materialPropertyBlock, float progress);
    }
}