using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class PersistentObject : MonoBehaviour
    {
        [SerializeField] Object persistentObjectPrefab;

        static bool hasSpawned = false;

        private void Awake()
        {
            if (hasSpawned)
            {
                return;
            }
            SpawnPersistentObjects();
            hasSpawned = true;
        }
        void SpawnPersistentObjects()
        {
            Object persistentObject = Instantiate(persistentObjectPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }
}
