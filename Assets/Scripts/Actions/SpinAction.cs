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
        if (totalSpinAmount >= 360f)
        {
            base.ActionComplete();
        }
        
    }
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return new List<GridPosition>{ unitGridPosition};
    }
    public override void TakeAction(GridPosition gridPosition,Action onSpinComplete)
    {
        totalSpinAmount =0;
        base.ActionStart(onSpinComplete);

    }
    public override string GetActionName()
    {
        return "Spin";
    }
    
}
