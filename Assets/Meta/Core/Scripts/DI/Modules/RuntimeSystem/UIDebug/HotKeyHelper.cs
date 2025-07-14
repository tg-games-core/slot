using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Debug
{
    public class HotKeyHelper
    {
        private readonly Dictionary<KeyCode, Action> _keyDictionary;

        public HotKeyHelper(Dictionary<KeyCode, Action> keyDictionary)
        {
            _keyDictionary = keyDictionary;
        }

        public void Tick()
        {
            foreach (var pair in _keyDictionary)
            {
                if (Input.GetKeyDown(pair.Key))
                {
                    _keyDictionary[pair.Key]?.Invoke();
                }
            }
        }
    }
}