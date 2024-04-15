using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShootAction : BaseAction
{
    private const int SHOTS_PER_ACTION = 3;
    private const int DAMAGE_PER_SHOT = 20;

    private int maxShootDistance=7;
    private float stateTimer;
    private Unit targetUnit;
    private bool canShoot;
    private float rotationSpeed = 15f;
    public event EventHandler<OnShootEventArgs> OnShoot;
    private int shotsPerAction = SHOTS_PER_ACTION;
    private int damagePerShot = DAMAGE_PER_SHOT;
    private float fireDelay =0.2f;
    private float timer = 0;
    

    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }

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
                if (targetUnit == null)
                {
                    canShoot = false;
                    shotsPerAction = SHOTS_PER_ACTION;
                    timer = 0;
                }
                if (canShoot&&timer>fireDelay) 
                {
                    
                    Shoot();
                    OnShoot?.Invoke(this,new OnShootEventArgs { targetUnit=targetUnit,shootingUnit=unit});
                    shotsPerAction--;
                    timer = 0;
                }
                if (shotsPerAction == 0)
                {
                    canShoot = false;
                    shotsPerAction = SHOTS_PER_ACTION;
                    timer = 0;
                }
                timer += Time.deltaTime;
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
                    float shootingStateTime = 2f;
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
        targetUnit.Damage(damagePerShot);
    }
    public override void TakeAction(GridPosition gridPosition, Action onShootComplete)
    {
        targetUnit =LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.Aiming;
        float aimingStateTime = 0.5f;
        stateTimer = aimingStateTime;
        canShoot = true;
        base.ActionStart(onShootComplete);
    }
    public override string GetActionName()
    {
        return "Shoot";
    }
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition);
    }
    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
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
    public Unit GetTargetUnit()
    {
        return targetUnit;
    }
    public int GetMaxShootDistance()
    {
        return maxShootDistance;
    }
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        float hpNormalized = targetUnit.GetHealthNormalized();
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 100+ Mathf.RoundToInt((1-hpNormalized)*100f),
        };
    }
    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList(gridPosition).Count;
    }
}
