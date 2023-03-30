using System;
using UnityEngine;

namespace NoGround.Utility
{
    public class CountdownTrigger
    {
        public event Action OnTriggered;
        public event Action OnDisable;
        public event Action<float> OnTickUpdate;

        public bool IsEnabled { get; private set; }

        private float timeToActivate = 1f;
        private float currentTime = 0f;

        public CountdownTrigger(float timeToActivate)
        {
            this.timeToActivate = timeToActivate;
        }

        /// <summary>
        /// Enables the countdown.
        /// </summary>
        public void Enable()
        {
            // Debug.Log("Enabling countdown trigger");
            IsEnabled = true;
        }

        /// <summary>
        /// Stops the countdown. Can trigger <see cref="CountdownTrigger.OnDisable"/> if countdown was active.
        /// </summary>
        public void Disable()
        {
            //Debug.Log("Disabling countdown trigger");

            if (!IsEnabled)
                return;

            OnDisable?.Invoke();
            Reset();
        }

        private void Reset()
        {
            IsEnabled = false;
            currentTime = 0f;
        }

        /// <summary>
        /// Updates the clock.
        /// </summary>
        /// <param name="deltaTime">Time that passed since last update</param>
        public void Update(float deltaTime)
        {
            if (IsEnabled)
            {
                currentTime = Mathf.Min(timeToActivate, currentTime + deltaTime);
                OnTickUpdate?.Invoke(currentTime / timeToActivate);

                if (currentTime >= timeToActivate)
                {
                    OnTriggered?.Invoke();
                    Reset();
                }
            }
        }
    }
}