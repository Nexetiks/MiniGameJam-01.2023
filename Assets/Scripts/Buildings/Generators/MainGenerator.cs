using System.Collections;
using NoGround.Pulses;
using UnityEngine;
using Utility;

namespace NoGround.Buildings.Generator
{
    public class MainGenerator : Generator
    {
        [field: SerializeField]
        protected override float TickRate { get; set; }
        [field: SerializeField]
        protected override float PulseSpeed { get; set; }

        [SerializeField]
        private CirclePulse pulsePrefab;
        [SerializeField]
        private AudioClipRandomizer surgeAudio;
        [SerializeField]
        private AudioClipRandomizer powerOnAudio;
        public uint Score = 1;
        public int Damage = 10;

        protected override void TickExecute()
        {
            CirclePulse circlePulse = Instantiate(pulsePrefab, transform.position, Quaternion.identity);
            circlePulse.SetUp(PulseSpeed, IsFriendly, Score, Damage);
            GameManager.Instance.AddScore(1);
            circlePulse.Tick();
            surgeAudio.Play();
        }

        protected override void OnTicking()
        {
            StartCoroutine(PowerOnAudio());
        }

        IEnumerator PowerOnAudio()
        {
            // This may be taken from the code
            float powerOnAudioLength = 1.5f;
            float timeToPowerUp = TickRate - powerOnAudioLength;

            if (timeToPowerUp > 0)
            {
                yield return new WaitForSeconds(timeToPowerUp);
                powerOnAudio.Play();
            }
        }

        protected override void TakePulseFromOtherGenerator()
        {
        }

        public override void OnBuildingBuilded()
        {
            Ticking();
        }
    }
}