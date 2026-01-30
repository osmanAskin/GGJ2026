using System;
using UnityEngine;
using Runtime.Enums;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public struct GameSoundData
    {
        public GameSoundType gameSoundType;
        public AudioClip[] clips;

        [Range(0f, 1f)] public float volume;

        public bool hasRandomPitch;
        public Vector2 pitchRange;

        public bool hasGlissando;
    }
}