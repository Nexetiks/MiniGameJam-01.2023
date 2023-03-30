using NoGround;
using NoGround.Buildings;
using NoGround.Pulses;
using UnityEngine;

namespace Buildings.WindWallBuildings
{
    public enum Destination
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }

    public class WindWallBuilding : Building, ITakePulse
    {
        public float MaxDistance;
        public int Damage;
        public float Width;
        public float Speed;
        public uint ScorePerTick = 1;

        [SerializeField]
        private WindWall wallprefab;

        public override void OnBuildingBuilded()
        {
        }

        bool ITakePulse.IsTakingPulsePossibleAtTheMoment
        {
            get => true;
            set { }
        }

        private void Start()
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
            WindWall wallInstance = Instantiate(wallprefab, transform.position, Quaternion.identity);
            wallInstance.SetUpAndStart(MaxDistance, (Destination)Random.Range(0, 3), Damage, Width, Speed, IsFriendly, ScorePerTick);
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }
    }
}