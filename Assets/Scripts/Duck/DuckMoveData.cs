
using UnityEngine;

[CreateAssetMenu(menuName = "Create DuckMoveData", fileName = "DuckMoveData", order = 0)]
public class DuckMoveData : ScriptableObject
{
    [SerializeField] private bool isStopped;
    
    public bool IsStopped
    {
        get => isStopped;
        set => isStopped = value;
    }

    
}