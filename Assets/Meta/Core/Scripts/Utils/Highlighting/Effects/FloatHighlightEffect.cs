using System;
using UnityEngine;

namespace Core.Highlighting
{
    public class FloatHighlightEffect : MaterialHighlightEffect<float>
    {
        public FloatHighlightEffect(int id, float from, float to) : base(id, from, to) { }

        protected override Func<float, float, float, float> Lerp
        {
            get => Mathf.Lerp;
        }

        protected override void Set(MaterialPropertyBlock materialPropertyBlock, float value)
        {
            materialPropertyBlock.SetFloat(_id, value);
        }
    }
}