using System;
using System.Collections;
using NoGround.Character;
using NoGround.Enums;
using UnityEngine;

namespace NoGround.Buildings
{
    public abstract class Building : MonoBehaviour
    {
        public event Action OnBuildingFinished;
        public static event Action OnAttack;
        public bool IsActiveAtStartOfTheGame = false;
        public bool IsFriendly = false;

        [SerializeField]
        protected float TimeToBuild;
        [SerializeField]
        private float timeLeftToDoneBuilding;
        [SerializeField]
        private float maxPlayerDistanceToBuild;
      //  [SerializeField]
       // private float minimumPlayerDistanceToBuild;
        [SerializeField]
        protected float WaitForAfterSpawn = 3f;

        private BuildingState buildingState;

        public float TimeLeftToDoneBuildingTheBuilding => timeLeftToDoneBuilding;
        public float MaxPlayerDistanceToBuild => maxPlayerDistanceToBuild;

        private void Awake()
        {
            StartBuildingTheBuilding();
        }

        private void OnEnable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AllBuildings.Add(this);
            }
        }

        private void OnDisable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AllBuildings.Remove(this);
            }
        }

        public void SelectBuildingToBuild()
        {
            UnSelectEveryBuilding();
            buildingState = BuildingState.Selected;
        }

        public void UnSelectBuilding()
        {
            buildingState = BuildingState.NoState;
        }

        private void UnSelectEveryBuilding()
        {
            foreach (Building building in GameManager.Instance.AllBuildings)
            {
                building.UnSelectBuilding();
            }
        }

        public void StartBuildingTheBuilding()
        {
            if (IsActiveAtStartOfTheGame == true)
            {
                gameObject.AddComponent<MeshCollider>();
                OnBuildingBuilded();
                OnBuildingFinished?.Invoke();
                return;
            }

            StartCoroutine(BuildBuilding());
        }

        public abstract void OnBuildingBuilded();

        private IEnumerator BuildBuilding() // dodac static do gracza
        {
            timeLeftToDoneBuilding = TimeToBuild;
            buildingState = BuildingState.WaitingForPlayerExit; 
            // Debug.Log("WaitingForPlayerExit");
            //yield return new WaitUntil(() => CheckMinimumPlayerDistance() == true); // wait until player distance > .....

            buildingState = BuildingState.BeingBuild;
            // Debug.Log("BeingBuild");
            yield return BuildingBuilding();

            buildingState = BuildingState.Build;
//            Debug.Log("Build");
            gameObject.AddComponent<MeshCollider>();
            GameManager.Instance.BuildBuildingAtEndOfState();
            yield return new WaitForSeconds(WaitForAfterSpawn);
            OnBuildingBuilded();
            OnBuildingFinished?.Invoke();
        }

        private IEnumerator BuildingBuilding()
        {
            while (timeLeftToDoneBuilding > 0)
            {
                if (CheckMaximumPlayerDistance())
                {
                    timeLeftToDoneBuilding -= Time.deltaTime;
                }
                else if (timeLeftToDoneBuilding < TimeToBuild)
                {
                    timeLeftToDoneBuilding += Time.deltaTime;
                }

                if (timeLeftToDoneBuilding < 0)
                {
                    timeLeftToDoneBuilding = 0;
                }

                yield return null;
            }
        }

        /*private bool CheckMinimumPlayerDistance()
        {
            if (minimumPlayerDistanceToBuild * minimumPlayerDistanceToBuild <= (gameObject.transform.position - PlayerCharacter.Instance.transform.position).sqrMagnitude)
            {
                return true;
            }

            timeLeftToDoneBuilding -= Time.deltaTime;
            return false;
        }*/

        public bool CheckMaximumPlayerDistance()
        {
            if (maxPlayerDistanceToBuild * maxPlayerDistanceToBuild > (gameObject.transform.position - PlayerCharacter.Instance.transform.position).sqrMagnitude)
            {
                return true;
            }

            return false;
        }
    }
}