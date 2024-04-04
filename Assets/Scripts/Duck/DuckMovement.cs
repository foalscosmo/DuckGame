using System;
using DG.Tweening;
using UnityEngine;

namespace Duck
{
    public class DuckMovement : MonoBehaviour
    {
        [SerializeField] private Transform[] alignPoints; // Array of alignment points for ducks
        [SerializeField] private float movementDuration; // Duration of duck movement
        [SerializeField] private Ducks ducks; // Reference to the Ducks script
        public event Action OnDuckAlign; // Event triggered when ducks are aligned
        [SerializeField] private DuckMoveData data;


        private void Awake()
        {
            data.IsStopped = false;
        }

        // Method to move ducks smoothly to align points
        public void MoveDucksSmoothly()
        {
            for (var i = 0; i < alignPoints.Length; i++)
            {
                var duckIndex = i; // Store current duck index
                DOTween.Sequence() // Create a DOTween sequence for smooth movement
                    .Append(ducks.ActiveDucks[duckIndex].transform.DOMove(alignPoints[duckIndex].position, movementDuration).SetEase(Ease.Linear)) // Move duck to alignment point
                    .OnComplete(() => // Callback when movement completes
                    {
                        if (duckIndex == alignPoints.Length - 1) // If it's the last duck
                        {
                            OnDuckAlign?.Invoke(); // Trigger alignment event
                            data.IsStopped = true;
                        }
                    });
            }
        }
    }
}

