using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NoGround
{
    [Serializable]
    public class Spawner<T> where T : MonoBehaviour
    {
        [SerializeField]
        private List<T> prefabs;

        public List<T> GetAllOptions() => prefabs;

        public T SpawnRandom(Vector3 position, Quaternion rotation)
        {
            return SpawnOption(Random.Range(0, prefabs.Count - 1), position, rotation);
        }

        public T SpawnOption(int id, Vector3 position, Quaternion rotation)
        {
            T prefab = prefabs[id];
            return MonoBehaviour.Instantiate(prefab, position, rotation);
        }
    }
}