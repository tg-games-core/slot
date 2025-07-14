using System;
using UnityEngine;

namespace Core
{
    [Serializable]
    public class SoundConfig
    {
        [SerializeField]
        private SoundType _audioType;

        [SerializeField]
        private AudioClip[] _clips;

        [SerializeField]
        private bool _isLinear;

        private int _index;
        
        public SoundType AudioType
        {
            get => _audioType;
        }

        public AudioClip Clip
        {
            get
            {
                AudioClip clip;

                if (_isLinear)
                {
                    clip = _clips[_index];
                    _index++;

                    if (_index >= _clips.Length)
                    {
                        _index = 0;
                    }
                }
                else
                {
                    clip = _clips.RandomElement();
                }

                return clip;
            }
        }
    }
}