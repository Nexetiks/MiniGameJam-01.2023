using Buildings.NormalBuildings.HolesBuilding.Buildings.NormalBuildings.HolesBuilding;
using NoGround;
using NoGround.Buildings;
using NoGround.Pulses;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Buildings.NormalBuildings.HolesBuilding
{
    public class HolesBuilding : Building, ITakePulse
    {
        [FormerlySerializedAs("hole")]
        [SerializeField]
        private Hole hole = null;
        [SerializeField]
        private Vector2 radius = new Vector2(10f, 10f);
        [SerializeField]
        private int numberOfHolesToSpawn = 3;
        private bool isSpawned = false;
        public uint ScorePerTick = 1;

        public override void OnBuildingBuilded()
        {
            Debug.Log("builded");
            RegisterInTakePulseList();
        }

        bool ITakePulse.IsTakingPulsePossibleAtTheMoment
        {
            get => true;
            set { }
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
            if (isSpawned)
            {
                return;
            }

            for (int i = 0; i < numberOfHolesToSpawn; i++)
            {
                Vector3 position = new Vector3(Random.Range(-radius.x, radius.x), Random.Range(-radius.y, radius.y), transform.position.z);
                Hole spawnedHole = Instantiate(hole, position, quaternion.identity, this.transform);
                spawnedHole.SetUp(IsFriendly, ScorePerTick);
                isSpawned = true;
            }
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }
    }
}