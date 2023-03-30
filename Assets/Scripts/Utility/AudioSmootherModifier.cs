using System;
using NoGround.Character;
using UnityEngine;

namespace Utility
{
    public class AudioSmootherModifier : MonoBehaviour
    {
        [SerializeField] private new PlayerCharacter playerCharacter;
        [SerializeField] private AudioSource audio;
        [SerializeField][Tooltip("Delta time modifier for lerping the sound volume.")]
        private float soundVolumeChangeSpeed = 2f;
        private void Awake()
        {
            audio.volume = 0.0f;
        }

        private void Update()
        {
            audio.volume = Mathf.Lerp(audio.volume, playerCharacter.GetSpeedNormalized(), Time.deltaTime * soundVolumeChangeSpeed);
        }
    }
}