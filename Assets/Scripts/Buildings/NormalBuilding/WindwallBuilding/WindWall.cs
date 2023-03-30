using System;
using System.Collections;
using NoGround;
using NoGround.Character;
using NoGround.Pulses;
using UnityEngine;

namespace Buildings.WindWallBuildings
{
    public class WindWall : MonoBehaviour
    {
        public Vector2 windWallPosition;
        public float MaxDistance;
        public Destination Destination;
        public int Damage = 1;
        public float Width;
        public float Speed;
        public bool IsFreindly;
        public uint ScorePerTick;

        public void SetUpAndStart(float maxDistance, Destination destination, int damage, float width, float speed, bool isFriendly, uint scorePerTick)
        {
            MaxDistance = maxDistance;
            Destination = destination;
            Damage = damage;
            Width = width;
            Speed = speed;
            IsFreindly = isFriendly;
            ScorePerTick = scorePerTick;

            StartCoroutine(ExecutePulse());
        }

        private IEnumerator ExecutePulse()
        {
            windWallPosition = transform.position;
            OnExecuteStart();

            while (Vector2.Distance(windWallPosition, transform.position) <= MaxDistance)
            {
                windWallPosition = ChangePlaceOfWindwall(windWallPosition);

                if (CheckIfPlayerCrossWindWall())
                {
                    if (((ITakePulse)PlayerCharacter.Instance).IsTakingPulsePossibleAtTheMoment)
                    {
                        if (IsFreindly)
                        {
                            GameManager.Instance.AddScore(ScorePerTick);
                        }
                        else
                        {
                            PlayerCharacter.Instance.TakeDamage(Damage);
                        }

                        break;
                    }
                }

                OnExecutePulseUpdate();
                yield return null;
            }
        }

        public void Remove()
        {
            Destroy(gameObject);
        }

        protected virtual void OnExecuteStart()
        {
        }

        protected virtual void OnExecutePulseUpdate()
        {
        }

        private Vector2 ChangePlaceOfWindwall(Vector2 windwallP)
        {
            if (Destination == Destination.Up)
            {
                windwallP = windwallP + new Vector2(0, 1) * Time.deltaTime * Speed;
            }

            if (Destination == Destination.Down)
            {
                windwallP = windwallP + new Vector2(0, -1) * Time.deltaTime * Speed;
            }

            if (Destination == Destination.Right)
            {
                windwallP = windwallP + new Vector2(1, 0) * Time.deltaTime * Speed;
            }

            if (Destination == Destination.Left)
            {
                windwallP = windwallP + new Vector2(-1, 0) * Time.deltaTime * Speed;
            }

            return windwallP;
        }

        private bool CheckIfPlayerCrossWindWall()
        {
            var playerPosition = PlayerCharacter.Instance.GetPosition();

            if (Destination == Destination.Up || Destination == Destination.Down)
            {
                if (Math.Abs(playerPosition.x) < (Math.Abs(windWallPosition.x) + Width / 2f) && Math.Abs(playerPosition.x) > (Math.Abs(windWallPosition.x) - Width / 2f))
                {
                    if (transform.position.y < playerPosition.y && playerPosition.y < windWallPosition.y)
                    {
                        if (Math.Abs(windWallPosition.y) - Math.Abs(playerPosition.y) <= 1f)
                        {
                            return true;
                        }
                    }
                    else if (transform.position.y > playerPosition.y && playerPosition.y > windWallPosition.y)
                    {
                        if (Math.Abs(windWallPosition.y) - Math.Abs(playerPosition.y) <= 1f)
                        {
                            return true;
                        }
                    }
                }
            }

            if (Destination == Destination.Left || Destination == Destination.Right)
            {
                if (Math.Abs(playerPosition.y) < (Math.Abs(windWallPosition.y) + Width / 2f) && Math.Abs(playerPosition.y) > (Math.Abs(windWallPosition.y) - Width / 2f))
                {
                    if (transform.position.x < playerPosition.x && playerPosition.x < windWallPosition.x)
                    {
                        if (Math.Abs(windWallPosition.x) - Math.Abs(playerPosition.x) <= 1f)
                        {
                            return true;
                        }
                    }
                    else if (transform.position.x > playerPosition.x && playerPosition.x > windWallPosition.x)
                    {
                        if (Math.Abs(windWallPosition.x) - Math.Abs(playerPosition.x) <= 1f)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}