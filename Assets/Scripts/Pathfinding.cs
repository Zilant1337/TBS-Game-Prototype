using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance;

    private const int STRAIGHT_MOVEMENT_COST = 10;
    private const int DIAGONAL_MOVEMENT_COST = 14;

    private int width;
    private int height;
    private float cellSize;

    private GridSystem<PathfindingNode> gridSystem;
    [SerializeField] private Transform gridObjectDebugPrefab;
    [SerializeField] private LayerMask obstaclesLayerMask;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one Pathfinding!!! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
    }
    public void Setup(int width, int height, float cellSize)
    {
        this.height = height;
        this.width = width;
        this.cellSize = cellSize;
        gridSystem = new GridSystem<PathfindingNode>(width, height, cellSize,
            (GridSystem<PathfindingNode> g, GridPosition gridPosition) => new PathfindingNode(gridPosition));
        //gridSystem.CreateDebugObjects(gridObjectDebugPrefab);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                float raycastOffset = 5f;
                if (Physics.Raycast(worldPosition + Vector3.down * raycastOffset, Vector3.up, raycastOffset * 2, obstaclesLayerMask))
                {
                    GetNode(x, z).SetIsWalkable(false);
                }
            }
        }
    }
    public List<GridPosition> FindPath(GridPosition startGridPos, GridPosition endGridPos,out int pathLength)
    {
        List<PathfindingNode> openList = new List<PathfindingNode>();
        List<PathfindingNode> closedList = new List<PathfindingNode>();
        PathfindingNode startNode = gridSystem.GetGridObject(startGridPos);
        PathfindingNode finalNode = gridSystem.GetGridObject(endGridPos);
        openList.Add(startNode);

        for (int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for (int z = 0; z < gridSystem.GetHeight(); z++)
            {
                GridPosition gridPos = new GridPosition(x, z);
                PathfindingNode pathNode = gridSystem.GetGridObject(gridPos);

                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetPrevPathfindingNode();
            }
        }
        startNode.SetGCost(0);
        startNode.SetHCost(CalculateHeuristicDistance(startGridPos, endGridPos));
        startNode.CalculateFCost();
        while (openList.Count > 0)
        {
            PathfindingNode currentNode = GetLowestFCostPathNodes(openList);

            if (currentNode == finalNode)
            {
                pathLength = finalNode.GetFCost();
                return CalculatePath(finalNode);
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);
            foreach(PathfindingNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode))
                {
                    continue;
                }
                if (!neighbourNode.IsWalkable())
                {
                    closedList.Add(neighbourNode);
                    continue;
                }
                int tentativeGCost = currentNode.GetGCost()+CalculateHeuristicDistance(currentNode.GetGridPosition(),neighbourNode.GetGridPosition());
                if (tentativeGCost < neighbourNode.GetGCost())
                {
                    neighbourNode.SetPrevPathfindingNode(currentNode);
                    neighbourNode.SetGCost(tentativeGCost);
                    neighbourNode.SetHCost(CalculateHeuristicDistance(neighbourNode.GetGridPosition(), endGridPos));
                    neighbourNode.CalculateFCost();
                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }
        pathLength= 0;
        return null;
    }
    
    private PathfindingNode GetNode(int x, int z)
    {
        return gridSystem.GetGridObject(new GridPosition(x, z));
    }
    private List<PathfindingNode> GetNeighbourList(PathfindingNode currentNode)
    {
        List<PathfindingNode>neighbourList=new List<PathfindingNode>();

        GridPosition gridPosition=currentNode.GetGridPosition();

        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                if(x==0 && z == 0)
                {
                    continue;
                }
                if(gridPosition.x + x>=0&& gridPosition.x + x<gridSystem.GetWidth() 
                    && gridPosition.z + z>=0 && gridPosition.z + z<gridSystem.GetHeight())
                {
                    neighbourList.Add(GetNode(gridPosition.x + x, gridPosition.z + z));
                }
            }
        }
        return neighbourList;
    }
    public int CalculateHeuristicDistance(GridPosition a, GridPosition b)
    {
        GridPosition gridPositionDistance = a-b;
        int xDistance= Mathf.Abs(gridPositionDistance.x);
        int zDistance = Mathf.Abs(gridPositionDistance.z);
        int remainder = Mathf.Abs(zDistance-xDistance);

        return DIAGONAL_MOVEMENT_COST*Mathf.Min(zDistance, xDistance)+STRAIGHT_MOVEMENT_COST* remainder;
    }
    public PathfindingNode GetLowestFCostPathNodes(List<PathfindingNode> pathfindingNodes)
    {
        PathfindingNode lowestFCostPathNode = pathfindingNodes[0];
        foreach (var pathNode in pathfindingNodes)
        {
            if (pathNode.GetFCost() <lowestFCostPathNode.GetFCost())
            {
                lowestFCostPathNode=pathNode;
            }
        }
        return lowestFCostPathNode;
    }
    private List<GridPosition> CalculatePath(PathfindingNode finalNode)
    {
        List<PathfindingNode> pathfindingNodeList = new List<PathfindingNode>();
        pathfindingNodeList.Add(finalNode);
        PathfindingNode currentNode = finalNode;
        while (currentNode.GetPrevPathfindingNode() != null)
        {
            pathfindingNodeList.Add(currentNode.GetPrevPathfindingNode());
            currentNode = currentNode.GetPrevPathfindingNode();
        }
        pathfindingNodeList.Reverse();
        List<GridPosition> gridPositionList= new List<GridPosition>();
        foreach(var pathNode in pathfindingNodeList)
        {
            gridPositionList.Add(pathNode.GetGridPosition());
        }
        return gridPositionList;
    }
    public void SetIsWalkableGridPosition(GridPosition gridPosition, bool IsWalkable)
    {
        gridSystem.GetGridObject(gridPosition).SetIsWalkable(IsWalkable);
    }
    public bool IsWalkableGridPosition(GridPosition gridPosition)
    {
        return gridSystem.GetGridObject(gridPosition).IsWalkable();
    }
    public bool IsReachableGridPosition(GridPosition startGridPosition,GridPosition endGridPosition)
    {
        return FindPath(startGridPosition,endGridPosition,out int pathLength)!=null;
    }
    public int GetPathLength(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        FindPath(startGridPosition, endGridPosition, out int pathLength);
        return pathLength;
    }
}

