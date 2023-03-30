using NoGround;
using NoGround.Buildings;
using NoGround.Character;
using NoGround.Pulses;
using UnityEngine;

namespace Buildings.NormalBuildings.ShootingBuilding
{
    public class ShootingBuilding : Building, ITakePulse
    {
        [SerializeField]
        private Arrow Arrow;
        private bool isTakePulseableAtTheMoment = true;
        public uint ScorePerTick = 1;

        public override void OnBuildingBuilded()
        {
            Debug.Log("builded");
            RegisterInTakePulseList();
        }

        bool ITakePulse.IsTakingPulsePossibleAtTheMoment
        {
            get => isTakePulseableAtTheMoment;
            set => isTakePulseableAtTheMoment = value;
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
            Arrow arrow = Instantiate(Arrow, transform.position, Quaternion.identity);
            arrow.SetUp(IsFriendly, ScorePerTick);
            arrow.rb.AddForce((PlayerCharacter.Instance.GetPosition() - arrow.transform.position).normalized * arrow.Speed);
            Debug.Log("Take Pulse");
        }

        public Vector3 GetPosition()
        {
            return gameObject.transform.position;
        }
    }
}