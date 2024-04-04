using System.Collections.Generic;
using Bucket;
using DG.Tweening;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    [SerializeField] private Buckets buckets; // Reference to the buckets
    [SerializeField] private Transform hintObj; // Reference to the hint object
    [SerializeField] private List<DragObject> dragObjects = new(); // List of draggable objects
    [SerializeField] private int hintIndex; // Index used for hint indication

    public bool StartTimer { get; set; } // Property to control timer activation
    private float afkTimer; // Timer for AFK (Away From Keyboard) check
    private const float AfkCheckInterval = 8f; // Interval for AFK check in seconds

    private void Awake()
    {
        hintObj.gameObject.SetActive(false); // Deactivate hint object on awake
    }

    // Update is called once per frame
    private void Update()
    {
        if (StartTimer) // If timer is activated
        {
            afkTimer += Time.deltaTime; // Increment AFK timer
            if (!(afkTimer >= AfkCheckInterval)) return; // If not reached check interval, exit
            CheckAfk(); // Otherwise, perform AFK check
        }
        else 
        {
            afkTimer = 0; // Reset AFK timer if timer is not started
        }
    }

    // Method to perform AFK check
    private void CheckAfk()
    {
        if (!StartTimer) return; // If timer is not started, exit
        afkTimer = 0f; // Reset AFK timer
        foreach (var t in dragObjects) // Loop through drag objects
        {
            if (t.gameObject.layer == hintIndex) // If object layer matches hint index
            {
                hintObj.transform.position = t.transform.position; // Set hint object position
                hintObj.gameObject.SetActive(true); // Activate hint object
            }
        }

        foreach (var t in buckets.ActiveBuckets) // Loop through active buckets
        {
            if (t.gameObject.layer == hintIndex) // If bucket layer matches hint index
            {
                hintObj.transform.DOMove(t.transform.position, 2f) // Move hint object to bucket position
                    .OnComplete(() => hintObj.gameObject.SetActive(false)); // Deactivate hint object after animation
            }
        }
    }

    // Method to activate the timer
    public void ActivateTimer()
    {
        StartTimer = true; // Set StartTimer property to true
    }
    
    // Method to handle hint index
    public void HintIndexHandler()
    {
        afkTimer = 0; // Reset AFK timer

        foreach (var obj in dragObjects) // Loop through drag objects
        {
            if (obj.IsSnapped == false) // If object is not snapped
            {
                hintIndex = obj.gameObject.layer; // Set hint index to object layer
                break; // Exit loop
            }
        }
        
        if (hintIndex > 8) // If hint index exceeds maximum value
        {
            hintIndex = 6; // Set hint index to a default value
        }
    }
}
