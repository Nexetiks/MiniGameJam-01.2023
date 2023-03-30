using UnityEngine;

namespace NoGround.Pulses
{
    public class ParticlePulse : CirclePulse
    {
        [SerializeField]
        protected ParticleSystem particleSystem;

        public override void Tick()
        {
            base.Tick();
            SetParticlesSpeed(particleSystem);
        }

        private void SetParticlesSpeed(ParticleSystem particleSystem)
        {
            var main = particleSystem.main;
            main.startSpeed = ExpandSpeed;
            ParticleSystem.SubEmittersModule subEmitters = particleSystem.subEmitters;
            for (int i = 0; i < subEmitters.subEmittersCount; i++)
            {
                ParticleSystem subSystem = subEmitters.GetSubEmitterSystem(i);
                SetParticlesSpeed(subSystem);
            }
        }
    }
}