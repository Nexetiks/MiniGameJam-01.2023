using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace NoGround.Buildings
{
    [Serializable]
    public class BuildScreenLogic
    {
        [SerializeField]
        private List<Building> buildingPrefabs;
        private int numberOfOptions = 2;
        private List<Building> currentOptions = new List<Building>();
        private Vector3 buildPosition;
        private Action buildingPlaced;
        public BuildScreen Screen { get; set; }

        public void Setup(Vector3 buildPosition, Action buildingPlaced)
        {
            this.buildPosition = buildPosition;
            this.buildingPlaced = buildingPlaced;
        }

        public List<Building> GetRandomBuildings()
        {
            currentOptions.Clear();

            if (buildingPrefabs.Count == 0)
                return null;

            if (buildingPrefabs.Count < numberOfOptions)
            {
                currentOptions = new() { buildingPrefabs[0], buildingPrefabs[0] };
            }
            else
            {
                List<Building> prefabsRange = new();

                // Copy the list of prefabs so we have list from which we can remove elements
                for (int i = 0; i < buildingPrefabs.Count; ++i)
                {
                    prefabsRange.Add(buildingPrefabs[i]);
                }

                for (int i = 0; i < numberOfOptions; i++)
                {
                    int prefabId = Random.Range(0, prefabsRange.Count - 1);
                    currentOptions.Add(prefabsRange[prefabId]);
                    prefabsRange.RemoveAt(prefabId);
                }
            }

            return currentOptions;
        }

        public void BuildOption(int id)
        {
            Object.Instantiate((Object)currentOptions[id], buildPosition, Quaternion.identity);
            buildingPlaced?.Invoke();
            Screen.Hide();
        }
    }
}