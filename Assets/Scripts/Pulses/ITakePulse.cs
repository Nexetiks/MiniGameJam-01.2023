using UnityEngine;

namespace NoGround.Pulses
{
    public interface ITakePulse
    {
        public bool IsTakingPulsePossibleAtTheMoment { get; protected set; }
        public void RegisterInTakePulseList();
        public void UnRegisterInTakePulseList();
        public void TakePulse();
        public Vector3 GetPosition();
    }
}