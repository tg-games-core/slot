﻿using UnityEngine;

namespace Core.UI
{
    public static class RectTransformExtensions
    {
        public static void SetViewportPosition(this RectTransform rect, Vector2 viewportPosition)
        {
            rect.anchorMax = viewportPosition;
            rect.anchorMin = viewportPosition;

            rect.anchoredPosition = viewportPosition;
        }
    }
}