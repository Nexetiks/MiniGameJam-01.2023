using System;
using UnityEngine;

namespace NoGround.Character
{
    [Serializable]
    public class HitPoints
    {
        public delegate void DamageTaken(float damage, float remainingHitPoints);
        public DamageTaken OnDamageTaken;
        public delegate void HitPointsDepleted();
        public HitPointsDepleted OnHitPointsDepleted;

        [SerializeField]
        [Tooltip("Starting hit points of the character")]
        private int startingValue = 10;
        [field: SerializeField]
        public int Value { get; private set; }

        public HitPoints(int startingValue)
        {
            this.startingValue = startingValue;
            Reset();
        }

        public void Reset()
        {
            Value = startingValue;
        }

        /// <summary>
        /// Reduces Hit Points.
        /// </summary>
        /// <param name="damage">Amount of damage that will be subtracted from the hit points.</param>
        /// <returns>False if hit points are depleted, true otherwise</returns>
        public bool TakeDamage(int damage)
        {
            Value = Mathf.Max(0, Value - damage);
            OnDamageTaken?.Invoke(damage, Value);

            if (Value == 0)
            {
                OnHitPointsDepleted?.Invoke();
                return false;
            }

            return true;
        }
    }
}