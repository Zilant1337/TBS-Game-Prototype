using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShootAction : BaseAction
{
    private int maxShootDistance=7;
    private float stateTimer;
    private Unit targetUnit;
    private bool canShoot;
    private float rotationSpeed = 15f;

    private enum State
    {
        Aiming, Shooting, Finishing
    }
    private State state;

    private void Awake()
    {
        base.Awake();
        actionPointCost = 1;
    }
    private void Update()
    {
        if (!isActive)
            return;
        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.Aiming:
                Vector3 moveDirection = (targetUnit.GetWorldPosition() - transform.position).normalized;
                transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);
                break;
            case State.Shooting:
                if (canShoot) Shoot();
                break;
            case State.Finishing:
                break;
        }

        if (stateTimer <= 0)
        {
            NextState();
        }

    }
    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                {
                    state = State.Shooting;
                    float shootingStateTime = 0.1f;
                    stateTimer = shootingStateTime;
                }
                break;
            case State.Shooting:
                {
                    state = State.Finishing;
                    float finishingStateTime = 0.5f;
                    stateTimer = finishingStateTime;
                }
                break;
            case State.Finishing:
                {
                    base.ActionComplete();
                }
                break;
        }

    }
    private void Shoot()
    {
        targetUnit.Damage();
    }
    public override void TakeAction(GridPosition gridPosition, Action onShootComplete)
    {
        base.ActionStart(onShootComplete);
        targetUnit =LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.Aiming;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;
        canShoot = true;
    }
    public override string GetActionName()
    {
        return "Shoot";
    }
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                if (unitGridPosition == testGridPosition)
                {
                    continue;
                }
                if (!LevelGrid.Instance.HasUnitOnGridPosition(testGridPosition))
                {
                    continue;
                }
                if (LevelGrid.Instance.HasUnitOnGridPosition(testGridPosition) && 
                    LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition).GetIsEnemy()==this.GetComponent<Unit>().GetIsEnemy())
                {
                    continue;
                }
                int testDistance =Mathf.Abs(x)+Mathf.Abs(z);
                if (testDistance > maxShootDistance)
                {
                    continue;
                }
                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }
}
