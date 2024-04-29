using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ShootAction;

public class SwordAction : BaseAction
{
    private int maxAttackDistance = 1;
    private State state;
    private float stateTimer;
    private Unit targetUnit;
    private float rotationSpeed= 15f;

    public event EventHandler OnSwordActionStart;
    public event EventHandler OnSwordActionComplete;
    public static event EventHandler OnAnySwordHit;

    private enum State
    {
        SwingBeforeHit,SwingAfterHit,
    }

    private void Awake()
    {
        base.Awake();
        actionPointCost = 1;
    }
    private void Update()
    {
        if (!isActive)
        {
            return;
        }
        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.SwingBeforeHit:
                Vector3 moveDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);
                break;
            case State.SwingAfterHit:
                break ;
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
            case State.SwingBeforeHit:
                {
                    state = State.SwingAfterHit;
                    float postHitStateTime = 0.2f;
                    stateTimer = postHitStateTime;
                    targetUnit.Damage(100);
                    OnAnySwordHit?.Invoke(this, EventArgs.Empty);
                }
                break;

            case State.SwingAfterHit:
                {
                    OnSwordActionComplete?.Invoke(this,EventArgs.Empty);
                    base.ActionComplete();
                }
                break;
        }

    }
    public override string GetActionName()
    {
        return "Sword";
    }

    public override int GetActionPointCost()
    {
        return base.GetActionPointCost();
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 200
        };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
    List<GridPosition> validGridPositionList = new List<GridPosition>();
    GridPosition unitGridPosition = unit.GetGridPosition();
    for (int x = -maxAttackDistance; x <= maxAttackDistance; x++)
    {
        for (int z = -maxAttackDistance; z <= maxAttackDistance; z++)
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
                LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition).GetIsEnemy() == this.GetComponent<Unit>().GetIsEnemy())
            {
                continue;
            }
            validGridPositionList.Add(testGridPosition);
        }
    }
    return validGridPositionList;
}

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.SwingBeforeHit;
        float beforeHitStateTime = 0.7f;
        stateTimer = beforeHitStateTime;

        OnSwordActionStart?.Invoke(this,EventArgs.Empty);

        ActionStart(onActionComplete);
    }

    public override string ToString()
    {
        return base.ToString();
    }
    public int GetMaxAttackDistance()
    {
        return maxAttackDistance;
    }
}
