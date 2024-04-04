using System;
using System.Collections;
using UnityEngine;

namespace Bucket
{
    public class BucketSpawner : MonoBehaviour
    {
        [SerializeField] private Transform[] spawnPoints; // Array of spawn points for buckets
        [SerializeField] private Buckets buckets; // Reference to the Buckets script
        public event Action OnBucketsSpawn; // Event triggered when buckets are spawned

        private void Awake()
        {
            foreach (var bucket in buckets.ActiveBuckets) bucket.SetActive(false); // Deactivate all buckets on awake
        }
    
        // Method to spawn buckets with a delay
        public void SpawnBuckets(int bucketIndex)
        {
            StartCoroutine(EnableBucketsWithDelay(bucketIndex));
        }

        private IEnumerator EnableBucketsWithDelay(int bucketIndex)
        {
            yield return new WaitForSecondsRealtime(0.3f); // Wait for a short delay before enabling buckets
            buckets.ActiveBuckets[bucketIndex].transform.position = spawnPoints[bucketIndex].position; // Set bucket position
            buckets.ActiveBuckets[bucketIndex].gameObject.SetActive(true); // Activate bucket
            if (bucketIndex == 2) OnBucketsSpawn?.Invoke(); // Trigger event when all buckets are spawned
        }

        // Method to disable all buckets with a delay
        public void DisableAllBuckets()
        {
            StartCoroutine(DisableBucketsWithDelay());
        }

        private IEnumerator DisableBucketsWithDelay()
        {
            yield return new WaitForSeconds(0.9f); // Wait for a short delay before disabling buckets
            foreach (var bucket in buckets.ActiveBuckets)
            {
                bucket.gameObject.SetActive(false); // Deactivate each bucket
            }
        }
    }
}