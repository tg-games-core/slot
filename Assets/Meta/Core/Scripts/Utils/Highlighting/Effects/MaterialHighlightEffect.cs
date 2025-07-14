using System;
using Core.Highlighting.Interfaces;
using UnityEngine;

namespace Core.Highlighting
{
    public abstract class MaterialHighlightEffect<T> : IHighlightEffect
    {
        protected readonly int _id;
        private readonly T _from;
        private readonly T _to;

        protected abstract Func<T, T, float, T> Lerp { get; }

        protected MaterialHighlightEffect(int id, T from, T to)
        {
            _to = to;
            _from = from;
            _id = id;
        }

        void IHighlightEffect.Reset(MaterialPropertyBlock materialPropertyBlock)
        {
            ((IHighlightEffect)this).Apply(materialPropertyBlock, 0f);
        }

        void IHighlightEffect.Apply(MaterialPropertyBlock materialPropertyBlock, float progress)
        {
            Set(materialPropertyBlock, Lerp(_from, _to, progress));
        }

        protected abstract void Set(MaterialPropertyBlock materialPropertyBlock, T value);
    }
}