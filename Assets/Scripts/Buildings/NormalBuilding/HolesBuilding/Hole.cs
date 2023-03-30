using NoGround;
using NoGround.Character;
using NoGround.Pulses;
using UnityEngine;

namespace Buildings.NormalBuildings.HolesBuilding
{
    namespace Buildings.NormalBuildings.HolesBuilding
    {
        public class Hole : MonoBehaviour
        {
            public int Damage = 10;
            public bool IsFriendly = false;
            public uint ScorePerTick = 1;

            public void SetUp(bool isFriendly, uint scorePerTick)
            {
                IsFriendly = isFriendly;
                ScorePerTick = scorePerTick;
            }

            private void OnTriggerEnter2D(Collider2D collider)
            {
                PlayerCharacter player = collider.GetComponent<PlayerCharacter>();

                if (player == null)
                {
                    player = collider.GetComponentInParent<PlayerCharacter>();
                }

                if (player == null)
                {
                    player = collider.GetComponentInChildren<PlayerCharacter>();
                }

                if (player != null)
                {
                    ITakePulse take = player as ITakePulse;

                    if (take != null)
                    {
                        if (take.IsTakingPulsePossibleAtTheMoment)
                        {
                            if (IsFriendly)
                            {
                                GameManager.Instance.AddScore(ScorePerTick);
                            }
                            else
                            {
                                player.TakeDamage(Damage);
                            }

                            Destroy(gameObject);
                        }

                        return;
                    }
                }
            }
        }
    }
}