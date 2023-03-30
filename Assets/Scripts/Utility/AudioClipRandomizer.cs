using System.Collections.Generic;

using UnityEngine;

namespace Utility
{
    public class AudioClipRandomizer : MonoBehaviour
    {
        public string Name = "";
        [SerializeField] private AudioSource source;
        [SerializeField] private List<AudioClip> audioClips;
        // [SerializeField] [MinMaxSlider(0, 1)] private Vector2 volume;
        [SerializeField] private Vector2 pitch;

        public void Play()
        {
            SetupAudio(GetRandomAudioClip());
            source.Play();
        }

        public void Stop()
        {
            source.Stop();
        }

        private AudioClip GetRandomAudioClip()
        {
            return audioClips[Random.Range(0, audioClips.Count)];
        }

        private void SetupAudio(AudioClip clip)
        {
            source.clip = clip;
            source.pitch = Random.Range(pitch.x, pitch.y);
        }
    }
}
