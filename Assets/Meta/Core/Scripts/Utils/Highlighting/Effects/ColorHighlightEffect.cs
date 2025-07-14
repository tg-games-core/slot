using System;
using UnityEngine;

namespace Core.Highlighting
{
    public class ColorHighlightEffect : MaterialHighlightEffect<Color>
    {
        public ColorHighlightEffect(int id, Color from, Color to) : base(id, from, to) { }

        protected override Func<Color, Color, float, Color> Lerp
        {
            get => Color.Lerp;
        }

        protected override void Set(MaterialPropertyBlock materialPropertyBlock, Color value)
        {
            materialPropertyBlock.SetColor(_id, value);
        }
    }
}