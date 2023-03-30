using System.Collections;
using Buildings.WindWallBuildings;
using NoGround;
using NoGround.Buildings;
using NoGround.Character;
using NoGround.Pulses;
using UnityEngine;

namespace Buildings.LuxBuildings
{
    public class LuxBuilding : Building, ITakePulse
    {
        public Destination Destination;
        public float Width;
        public float Range;
        public float LoadingTime;
        public uint Score;
        public int Damage = 10;

        public override void OnBuildingBuilded()
        {
        }

        bool ITakePulse.IsTakingPulsePossibleAtTheMoment
        {
            get => true;
            set { }
        }

        private void Awake()
        {
            RegisterInTakePulseList();
        }

        public void RegisterInTakePulseList()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.TakePulseList.Add(this);
                GameManager.Instance.AllBuildings.Add(this);
            }
        }

        public void UnRegisterInTakePulseList()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.TakePulseList.Remove(this);
                GameManager.Instance.AllBuildings.Remove(this);
            }
        }

        public void TakePulse()
        {
            Debug.Log("pulse");
            StartCoroutine(Loading());
        }

        private IEnumerator Loading()
        {
            Destination = (Destination)Random.Range(0, 3);
            yield return new WaitForSeconds(LoadingTime);
            MakeCollider();
        }

        private void MakeCollider()
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(GetCenter(), GetExten(), 360f);

            foreach (Collider2D collider in colliders)
            {
                PlayerCharacter player = collider.gameObject.GetComponent<PlayerCharacter>();

                if (player == null)
                {
                    player = collider.gameObject.GetComponentInChildren<PlayerCharacter>();
                }

                if (player == null)
                {
                    player = collider.gameObject.GetComponentInParent<PlayerCharacter>();
                }

                if (player != null)
                {
                    if (((ITakePulse)player).IsTakingPulsePossibleAtTheMoment)
                    {
                        if (IsFriendly)
                        {
                            GameManager.Instance.AddScore(Score);
                        }
                        else
                        {
                            player.TakeDamage(Damage);
                        }
                    }
                }
            }
        }

        private Vector2 GetCenter()
        {
            Vector2 position = this.transform.position;

            if (Destination == Destination.Up)
            {
                position = new Vector2(position.x, position.y + Range / 2);
            }
            else if (Destination == Destination.Down)
            {
                position = new Vector2(position.x, position.y - Range / 2);
            }
            else if (Destination == Destination.Left)
            {
                position = new Vector2(position.x - Range / 2, position.y);
            }
            else if (Destination == Destination.Right)
            {
                position = new Vector2(position.x + Range / 2, position.y);
            }

            return position;
        }

        private Vector2 GetExten()
        {
            Vector3 Ext = Vector3.zero;

            if (Destination == Destination.Up || Destination == Destination.Down)
            {
                Ext = new Vector3(Width / 2f, Range);
            }
            else if (Destination == Destination.Left || Destination == Destination.Right)
            {
                Ext = new Vector3(Range, Width / 2f);
            }

            return Ext;
        }

        /*void OnDrawGizmos()
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(GetCenter(), GetExten());
        }*/

        public Vector3 GetPosition()
        {
            return gameObject.transform.position;
        }
    }
}