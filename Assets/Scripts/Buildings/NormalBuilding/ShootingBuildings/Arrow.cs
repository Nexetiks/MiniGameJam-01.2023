using System.Collections;
using NoGround;
using NoGround.Character;
using NoGround.Pulses;
using UnityEngine;

namespace Buildings.NormalBuildings.ShootingBuilding
{
    public class Arrow : MonoBehaviour
    {
        public ProjectileVisuals visuals;
        public float Speed;
        public int Damage = 1;
        public float TimeToLive = 5f;
        public Rigidbody2D rb;
        public bool IsFriendly = false;
        public uint ScorePerTick = 1;

        private void Awake()
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody2D>();
            }

            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody2D>();
            }
        }

        public void SetUp(bool isFriendly, uint scorePerTick)
        {
            IsFriendly = isFriendly;
            ScorePerTick = scorePerTick;
            GameManager.Instance.AddScore(1);
        }

        private void Start()
        {
            StartCoroutine(Autodestruct());
            visuals.transform.SetParent(null);
            //Destroy(gameObject, 15f);
            //Destroy(visuals.gameObject, 15f);
        }

        IEnumerator Autodestruct()
        {
            yield return new WaitForSeconds(TimeToLive);
            Remove();
        }

        /// <summary>
        /// windwall
        /// lux
        /// budynek spamiacy 3 kolka
        /// klony wierzyczek ale ich przeciwienstwa
        /// </summary>
        /// <param name="collider"></param>
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
                    //if (take.IsTakingPulsePossibleAtTheMoment)
                    {
                        if (IsFriendly)
                        {
                            GameManager.Instance.AddScore(ScorePerTick);
                        }
                        else
                        {
                            player.TakeDamage(Damage);
                        }
                    }
                    //else
                    //{
                    //    return;
                    //}
                }
            }

            Remove();
        }

        private void LateUpdate()
        {
            visuals.transform.position = new Vector3(transform.position.x, 0, transform.position.y);
        }

        public void Remove()
        {
            Destroy(visuals.gameObject);
            Destroy(gameObject);
        }
    }
}