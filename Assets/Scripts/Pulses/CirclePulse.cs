using System.Collections;
using System.Collections.Generic;
using NoGround.Character;
using UnityEngine;

namespace NoGround.Pulses
{
    public class CirclePulse : Pulse
    {
        [SerializeField]
        protected float Radius = 0f;
        [field: SerializeField]
        protected override float ExpandSpeed { get; set; }
        public bool IsFriendly = false;
        public uint Score = 1;
        public int Damage = 10;

        public float CurrentRadius => Radius;

        public void SetUp(float speed, bool isFriendly, uint score, int damage)
        {
            ExpandSpeed = speed;
            IsFriendly = isFriendly;
            Score = score;
            Damage = damage;
        }

        public override void Tick()
        {
            StartCoroutine(ExpandShape());
        }

        private IEnumerator ExpandShape()
        {
            List<ITakePulse> copyList = new List<ITakePulse>();
            copyList.AddRange(GameManager.Instance.TakePulseList);

            while (copyList.Count >= 1)
            {
                CheckList(ref copyList);
                Radius += Time.deltaTime * ExpandSpeed;
                yield return null;
            }

            Remove();
        }

        public void Remove()
        {
            Destroy(gameObject);
        }

        protected void CheckList(ref List<ITakePulse> TakePulsList)
        {
            for (int i = 0; i < TakePulsList.Count; i++)
            {
                Vector3 position = TakePulsList[i].GetPosition();
                float distance = Vector3.Distance(position, gameObject.transform.position);

                if (distance < Radius)
                {
                    if (TakePulsList[i].IsTakingPulsePossibleAtTheMoment)
                    {
                        if (IsFriendly && TakePulsList[i].GetPosition() == PlayerCharacter.Instance.gameObject.transform.position)
                        {
                            GameManager.Instance.AddScore(Score);
                            return;
                        }

                        TakePulsList[i].TakePulse();
                    }

                    TakePulsList.RemoveAt(i);
                    i--;

                    if (TakePulsList.Count == 0)
                    {
                        return;
                    }
                }
            }
        }
    }
}