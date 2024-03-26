using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoveAction : BaseAction
{

    private Vector3 targetPosition;
    [SerializeField] private Animator unitAnimator;
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
            unitAnimator.SetBool("IsWalking", true);
        }
        else
        {
            unitAnimator.SetBool("IsWalking", false);
            isActive = false;
            onActionComplete();
        }

        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);
    }
    public override void TakeAction(GridPosition targetPosition, Action onMoveComplete)
    {
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(targetPosition);
        isActive = true;
        this.onActionComplete = onMoveComplete;
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
                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }
    public override string GetActionName()
    {
        return "Move";
    }
    /*public override int GetActionPointCost()
    {
        throw new NotImplementedException();
    }*/
}
