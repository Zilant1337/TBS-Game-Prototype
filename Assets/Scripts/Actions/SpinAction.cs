using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float spinAmount;
    private float totalSpinAmount;
    private void Awake()
    {
        base.Awake();
        actionPointCost = 1;
    }
    private void Update()
    {
        if (!isActive)
            return;
        
        spinAmount = 360f * Time.deltaTime;
        totalSpinAmount +=spinAmount;
        transform.eulerAngles += new Vector3(0, spinAmount, 0);
        if (totalSpinAmount > 360f)
        {
            isActive = false;
            onActionComplete();
            totalSpinAmount = 0;
        }
        
    }
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();
        validGridPositionList.Add(unitGridPosition);
        return validGridPositionList;
    }
    public override void TakeAction(GridPosition gridPosition,Action onSpinComplete)
    {   
        totalSpinAmount =0;
        isActive = true;
        this.onActionComplete = onSpinComplete;
    }
    public override string GetActionName()
    {
        return "Spin";
    }
    
}
