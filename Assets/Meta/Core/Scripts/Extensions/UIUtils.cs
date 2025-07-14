using UnityEngine;

namespace Core
{
    public static class UIUtils
    {
        private static readonly Vector2 ReferenceResolution = new Vector2(1080, 1920);
    
        private static Vector2? _offset = null;
        private static float? _buttomOffset = null;

        public static bool IsXDevice
        {
            get { return Offset != Vector2.zero; }
        }

        public static Vector2 Offset
        {
            get
            {
                if (!_offset.HasValue)
                {
                    _offset = GetScreenOffset();
                }
                return _offset.Value;
            }
        }

        public static float ButtomOffset
        {
            get
            {
                if (!_buttomOffset.HasValue)
                {
                    _buttomOffset = GetScreenButtonOffset();
                }
                return _buttomOffset.Value;
            }
        }

        public static Vector2 GetScreenOffset()
        {
            float ratio = (float)Screen.height / Screen.width;
        
            if (ratio >= 1.9f)
            {
                // S: пока подразумаваем, что на девайсах с таким аспектом есть моноброви
                return new Vector2(0, -120);
            }
            else
            {
                return Vector2.zero;
            }
        }

        public static float GetScreenButtonOffset()
        {
            float ratio = (float)Screen.height / Screen.width;

            if (ratio >= 1.9f)
            {
                // S: пока подразумаваем, что на девайсах с таким аспектом есть моноброви
                return 60f;
            }
            else
            {
                return 0f;
            }
        }


        public static float GetMatchWidthOrHeight()
        {
            float ratio = (float)Screen.height / Screen.width;
            // TODO: нужно подумать как скейлить юи под планшеты, поресерчить в гугле что то 
            return ratio < 1.5f ? 1 : 0;
        }
    }
}