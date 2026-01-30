using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
using Runtime.Enums;
using System.Linq;

namespace Runtime.Managers
{
    public class SoundManager : SingletonMonoBehaviour<SoundManager>
    {
        [Header("Default Audio Sources")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioSource glissandoAudioSource;
        [SerializeField] private AudioSource randomPitchAudioSource;

        [Header("Sound Collection")]
        [SerializeField] private CD_GameSound COLLECTION;

        [Header("Glissando Settings")]
        [SerializeField] private float glissandoDuration = 1f;
        [SerializeField] private float glissandoDefaultPitch = 1f;

        private Coroutine glissandoCoroutine;
        private Dictionary<GameSoundType, int> lastPlayedIndex = new Dictionary<GameSoundType, int>();

        public void PlaySound(GameSoundType type)
        {
            if (!SettingsManager.Instance.isSoundActive)
                return;

            var soundData = COLLECTION.gameSoundData.FirstOrDefault(x => x.gameSoundType == type);
            if (soundData.clips == null || soundData.clips.Length == 0)
                return;

            // Farklı bir index seç
            int lastIndex = lastPlayedIndex.ContainsKey(type) ? lastPlayedIndex[type] : -1;
            int newIndex;
            do
            {
                newIndex = Random.Range(0, soundData.clips.Length);
            } while (soundData.clips.Length > 1 && newIndex == lastIndex);

            lastPlayedIndex[type] = newIndex;
            AudioClip clip = soundData.clips[newIndex];

            if (soundData.hasGlissando)
            {
                if (glissandoCoroutine != null)
                    StopCoroutine(glissandoCoroutine);

                glissandoCoroutine = StartCoroutine(PlayGlissando(clip, soundData));
            }
            else if (soundData.hasRandomPitch)
            {
                float pitch = Random.Range(soundData.pitchRange.x, soundData.pitchRange.y);
                randomPitchAudioSource.pitch = pitch;
                randomPitchAudioSource.volume = soundData.volume;
                randomPitchAudioSource.PlayOneShot(clip);
            }
            else
            {
                audioSource.volume = soundData.volume;
                audioSource.PlayOneShot(clip);
            }
        }

        private IEnumerator PlayGlissando(AudioClip clip, GameSoundData data)
        {
            float elapsedTime = 0f;
            float initialPitch = glissandoAudioSource.pitch;
            float targetPitch = initialPitch + (data.pitchRange.y - data.pitchRange.x);

            glissandoAudioSource.pitch = initialPitch;
            glissandoAudioSource.volume = data.volume;
            glissandoAudioSource.PlayOneShot(clip);

            while (elapsedTime < glissandoDuration)
            {
                float t = elapsedTime / glissandoDuration;
                glissandoAudioSource.pitch = Mathf.Lerp(initialPitch, targetPitch, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            glissandoAudioSource.pitch = glissandoDefaultPitch;
        }
    }
}
