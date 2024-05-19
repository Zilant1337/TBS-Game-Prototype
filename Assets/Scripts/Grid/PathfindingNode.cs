
public class PathfindingNode
{
    private GridPosition gridPosition;
    private int gCost;
    private int hCost;
    private int fCost;
    private bool isWalkable=true;
    private PathfindingNode prevPathfindingNode;
    public PathfindingNode(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
    }

    public override string ToString()
    {
        return gridPosition.ToString();
    }
    public bool IsWalkable()
    {
        return isWalkable;
    }
    public int GetGCost()
    {
        return gCost;
    }
    public int GetHCost()
    {
        return hCost;
    }
    public int GetFCost()
    {
        return fCost;
    }
    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }
    public PathfindingNode GetPrevPathfindingNode()
    {
        return prevPathfindingNode;
    }
    public void SetIsWalkable(bool isWalkable)
    {
        this.isWalkable = isWalkable;
    }
    public void SetGCost(int gCost)
    {
        this.gCost = gCost;
    }
    public void SetHCost(int hCost)
    {
        this.hCost = hCost;
    }
    public void SetPrevPathfindingNode(PathfindingNode pathfindingNode)
    {
        this.prevPathfindingNode = pathfindingNode;
    }
    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
    public void ResetPrevPathfindingNode()
    {
        prevPathfindingNode = null;
    }
}
