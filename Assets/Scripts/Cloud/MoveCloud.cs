
using DG.Tweening;
using UnityEngine;

public class MoveCloud : MonoBehaviour
{
    [SerializeField] private Transform destinationPoint; // The point where the cloud moves towards.
    [SerializeField] private Transform respawnPoint; // The point where the cloud respawns after reaching its destination.
    [SerializeField] private int durationToPoint; // The time it takes for the cloud to move to the destination point.

    private void Start()
    {
        Move(); // Initiates the movement of the cloud.
    }

    // Function to handle what happens when the movement to the destination point is complete.
    private void OnMovementComplete()
    {
        transform.position = respawnPoint.position; // Sets the position of the cloud to the respawn point.
        Move(); // Initiates the movement of the cloud again.
    }

    // Function to move the cloud towards the destination point.
    private void Move()
    {
        transform.DOMoveX(destinationPoint.position.x, durationToPoint) // Moves the cloud towards the destination point along the x-axis.
            .SetEase(Ease.InOutQuad) // Sets the ease of the movement to InOutQuad for smoother motion.
            .OnComplete(OnMovementComplete); // Calls OnMovementComplete() when the movement is complete. 
    }
}
