using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoveAction : BaseAction
{
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;
    private Vector3 targetPosition;
    private float moveSpeed = 5.0f;
    private float stoppingDistance = 0.01f;
    private float rotationSpeed = 15f;
    [SerializeField] int maxMoveDistance=4;
    // Update is called once per frame
    protected override void Awake()
    {
        base.Awake();
        targetPosition = transform.position;
        actionPointCost = 1;
    }
    void Update()
    {
        if (!isActive)
            return;
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            transform.position += moveDirection * Time.deltaTime * moveSpeed;
        }
        else
        {
            OnStopMoving?.Invoke(this,EventArgs.Empty);
            base.ActionComplete();
        }

        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);
    }
    public override void TakeAction(GridPosition targetPosition, Action onMoveComplete)
    {
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(targetPosition);
        OnStartMoving?.Invoke(this, EventArgs.Empty);
        base.ActionStart(onMoveComplete);

    }
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList= new List<GridPosition>();
        GridPosition unitGridPosition=unit.GetGridPosition();
        for(int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for(int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x,z);
                GridPosition testGridPosition = unitGridPosition+offsetGridPosition;
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) 
                {
                    continue;
                }
                if(unitGridPosition==testGridPosition)
                {
                    continue;
                }
                if (LevelGrid.Instance.HasUnitOnGridPosition(testGridPosition))
                {
                    continue;
                }
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxMoveDistance)
                {
                    continue;
                }
                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }
    public override string GetActionName()
    {
        return "Move";
    }
}
