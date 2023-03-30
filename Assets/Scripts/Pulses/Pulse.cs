using UnityEngine;

namespace NoGround.Pulses
{
    public abstract class Pulse : MonoBehaviour
    {
        protected abstract float ExpandSpeed { get; set; }

        public abstract void Tick();

    }
}