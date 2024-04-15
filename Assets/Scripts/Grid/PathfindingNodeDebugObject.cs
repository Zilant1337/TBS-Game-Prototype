using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PathfindingNodeDebugObject : GridDebugObject
{
    [SerializeField] private TextMeshPro hCostText;
    [SerializeField] private TextMeshPro gCostText;
    [SerializeField] private TextMeshPro fCostText;
    private PathfindingNode pathfindingNode;
    public override void SetGridObject(object gridObject)
    {

        base.SetGridObject(gridObject);
        pathfindingNode = (PathfindingNode)gridObject;
    }
    protected override void Update()
    {
        base.Update();
        gCostText.text = pathfindingNode.GetGCost().ToString();
        hCostText.text = pathfindingNode.GetHCost().ToString();
        fCostText.text=pathfindingNode.GetFCost().ToString();
    }
}
