using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private int width;
    private int height;
    private float cellSize;
    private GridSystem<PathfindingNode> gridSystem;
    [SerializeField] private Transform gridObjectDebugPrefab;

    private void Awake()
    {
        gridSystem =new GridSystem <PathfindingNode>(10, 10, 2f, 
            (GridSystem<PathfindingNode> g, GridPosition gridPosition) => new PathfindingNode(gridPosition));
        gridSystem.CreateDebugObjects(gridObjectDebugPrefab);
    }
}
