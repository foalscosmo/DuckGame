using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening; 
using UnityEngine;

public class DragObject : MonoBehaviour
{
    private Camera mainCamera; // Reference to the main camera
    private Vector2 offset; // Offset between mouse position and object position
    [SerializeField] private Transform originalPosition; // Initial position of the object
    [SerializeField] private List<Transform> targetTransforms = new(); // List of target positions for snapping
    [SerializeField] private SpriteRenderer sr; // Reference to the sprite renderer component
    [SerializeField] private int index; // Index of the object
    [SerializeField] private SpriteRenderer duckFrame; // DucksFrames
    [SerializeField] private DuckMoveData data;
    public event Action OnCorrectSnap; // Event triggered when the object snaps correctly
    public event Action<int> OnObjGrab; // Event triggered when the object is grabbed
    public event Action<int> OnObjDrop; // Event triggered when the object is dropped
    public event Action<Transform> OnTransformChange; // Event triggered when the target transform changes
    
    public bool IsSnapped { get; private set; } // Boolean indicating whether the object is snapped to a target
    private bool isDragging; // Boolean indicating whether the object is currently being dragged
    private bool isAttached;

    private void Awake()
    {
        mainCamera = Camera.main; // Initialize the mainCamera reference
    }

    private void OnEnable()
    {
        IsSnapped = false; // Reset the IsSnapped flag when the object is enabled
    }

    // FixedUpdate is called at a fixed interval and is used for physics calculations
    private void FixedUpdate()
    {
        // If the object is being dragged, update its position based on mouse input
        if (isDragging)
        {
            var mousePosition = GetMousePosition();
            transform.position = mousePosition - offset;
        }
        // If the object is not being dragged and is at its original position, apply floating animation
        else if (!isDragging && transform.position == originalPosition.position)
        {
            Floating(transform.position);
        }
    }

    // Method to apply floating animation to the object
    private void Floating(Vector3 position)
    {
        transform.DOMove(position + Vector3.up * 0.1f, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.DOMove(position + Vector3.down * 0.1f, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                Floating(position);
            });
        });
    }

    // Method called when the mouse button is pressed on the object
    private void OnMouseDown()
    {
      
        if (!data.IsStopped) return;
        if(IsSnapped) return; // If the object is already snapped, return
        isDragging = true; // Set dragging flag to true
        isAttached = true;
        transform.DOKill(); // Stop any ongoing DOTween animations
        offset = GetMousePosition() - (Vector2)transform.position; // Calculate offset between mouse and object position
        transform.DOScale(1.1f, 0.2f); // Apply scale animation to indicate grabbing
        sr.sortingOrder = 4; // Set sorting order to bring the object to the front
        duckFrame.sortingOrder = 5;
        
        OnObjGrab?.Invoke(index); // Invoke OnObjGrab event with the object's index
    }

    // Method called when the mouse button is released
    private void OnMouseUp()
    {
        if (!isAttached) return;
        if (!data.IsStopped) return;
        if(IsSnapped) return; // If the object is already snapped, return
        isDragging = false; // Set dragging flag to false
        IsSnapped = false; // Reset the snapped flag
        transform.DOScale(1f, 0.2f); // Reset scale to normal
        OnObjDrop?.Invoke(index); // Invoke OnObjDrop event with the object's index
        // Loop through target transforms to check for snapping
        foreach (var target in from target in targetTransforms
                     let distanceToBucket = Vector2.Distance(transform.position, target.position)
                     where distanceToBucket < 3.0f
                     where gameObject.layer == target.gameObject.layer
                     select target) {
            var position1 = target.position;
            var position = new Vector3(position1.x, position1.y + 1.8f);
            transform.position = new Vector3(position.x, position.y + 1.8f);
            sr.sortingOrder = 1;
            duckFrame.sortingOrder = 2;
            OnTransformChange?.Invoke(target);
            IsSnapped = true; // Set snapped flag to true
            isAttached = false; 
            Floating(position); // Apply floating animation to the object
            OnCorrectSnap?.Invoke(); // Invoke OnCorrectSnap event
            break; // Exit the loop after snapping to the first valid target
        }
        // If the object is not snapped to any target, reset its position and sorting order
        if (!data.IsStopped) return;
        if (!IsSnapped)
        {
            transform.position = originalPosition.position;
            sr.sortingOrder = 1;
            duckFrame.sortingOrder = 2;
           isAttached = false; 
        }
    }
    
    // Method to get mouse position in world coordinates
    private Vector2 GetMousePosition()
    {
        return mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }
}