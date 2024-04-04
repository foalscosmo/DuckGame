using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Duck
{
    public class DuckSpawner : MonoBehaviour
    {
        [SerializeField] private Ducks ducks; // Reference to the Ducks script
        [SerializeField] private List<Transform> duckSpawnPoints; // List of spawn points for ducks
        public event Action OnDucksSpawn; // Event triggered when ducks are spawned

        private void Awake()
        {
            SpawnDucksToStartPoint(); // Spawn ducks to start points on awake
        }

        // Method to set active state of ducks
        public void SetActiveDucks()
        {
            foreach (var duck in ducks.ActiveDucks) duck.SetActive(true); // Activate all ducks
            OnDucksSpawn?.Invoke(); // Trigger ducks spawn event
        }

        // Method to disable ducks with a delay
        public void DisableDucks()
        {
            StartCoroutine(DisableDuckWithDelay());
        }

        private IEnumerator DisableDuckWithDelay()
        {
            yield return new WaitForSeconds(0.9f); // Wait for a short delay before disabling ducks
            foreach (var duck in ducks.ActiveDucks)
            {
                duck.gameObject.SetActive(false); // Deactivate each duck
            }
        }

        // Method to spawn ducks to their start points
        public void SpawnDucksToStartPoint()
        {
            for (var index = 0; index < ducks.ActiveDucks.Count; index++)
            {
                ducks.ActiveDucks[index].transform.position = duckSpawnPoints[index].position; // Set duck position to spawn point position
            }
        }
    }
}