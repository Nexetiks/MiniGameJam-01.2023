using System.Collections;
using NoGround.Pulses;
using UnityEngine;

namespace NoGround.Buildings.Generator
{
    public abstract class Generator : Building, ITakePulse
    {
        private bool isTakingPulsePossibleAtTheMoment;
        protected abstract float TickRate { get; set; }
        protected abstract float PulseSpeed { get; set; }

        protected void Ticking()
        {
            StartCoroutine(Tick());
            OnTicking();
        }

        protected virtual void OnTicking()
        {
        }

        private IEnumerator Tick()
        {
            yield return new WaitForSeconds(TickRate);
            TickExecute();
            Ticking();
        }

        private void Start()
        {
            RegisterInTakePulseList();
        }

        private void OnDestroy()
        {
            UnRegisterInTakePulseList();
        }

        protected abstract void TickExecute();

        protected abstract void TakePulseFromOtherGenerator();

        bool ITakePulse.IsTakingPulsePossibleAtTheMoment
        {
            get { return true; }
            set { }
        }

        public void RegisterInTakePulseList()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.TakePulseList.Add(this);
            }
        }

        public void UnRegisterInTakePulseList()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.TakePulseList.Remove(this);
            }
        }

        public void TakePulse()
        {
            TakePulseFromOtherGenerator();
        }

        public Vector3 GetPosition()
        {
            return gameObject.transform.position;
        }
    }
}