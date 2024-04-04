using System.Collections.Generic;
using Duck;
using UnityEngine;

namespace Managers
{
    public class DuckManager : MonoBehaviour
    {
        // Reference to the duck spawner
        [SerializeField] private DuckSpawner duckSpawner; // Reference to the duck spawner.
        
        // Reference to the duck movement
        [SerializeField] private DuckMovement duckMovement; // Reference to the duck movement.
        
        // List of snapped ducks
        [SerializeField] private List<DragObject> snappedDucks; // List of ducks that are snapped.
        
        // Property to access snapped ducks
        public List<DragObject> SnappedDucks => snappedDucks; // Accessor to the list of snapped ducks.
        
        // Property to access duck spawner
        public DuckSpawner DuckSpawner => duckSpawner; // Accessor to the duck spawner.
        
        // Property to access duck movement
        public DuckMovement DuckMovement => duckMovement; // Accessor to the duck movement.

        // Subscribe to events when the object is enabled
        private void OnEnable()
        {
            // Subscribe to the duck spawn event to move ducks smoothly
            duckSpawner.OnDucksSpawn += duckMovement.MoveDucksSmoothly;
        }

        // Unsubscribe from events when the object is disabled
        private void OnDisable()
        {
            // Unsubscribe from the duck spawn event to move ducks smoothly
            duckSpawner.OnDucksSpawn -= duckMovement.MoveDucksSmoothly;
        }
    }
}