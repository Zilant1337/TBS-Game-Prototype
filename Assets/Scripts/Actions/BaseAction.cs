using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    public static event EventHandler OnActionStart;
    public static event EventHandler OnActionEnd;


    protected Unit unit;
    protected bool isActive;
    protected Action onActionComplete;
    protected int actionPointCost;
    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }
    public abstract string GetActionName();
    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);
    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositions = GetValidActionGridPositionList();
        return validGridPositions.Contains(gridPosition);
    }
    public abstract List<GridPosition> GetValidActionGridPositionList();
    public virtual int GetActionPointCost()
    {
        return actionPointCost;
    }
    protected void ActionStart(Action onActionComplete)
    {
        isActive = true;
        this.onActionComplete = onActionComplete;
        OnActionStart?.Invoke(this, EventArgs.Empty);
    }
    protected void ActionComplete()
    {
        isActive = false;
        onActionComplete();
        OnActionEnd?.Invoke(this, EventArgs.Empty);
    }
    public Unit GetUnit()
    {
        return unit;
    }

    public EnemyAIAction GetBestEnemyAIAction()
    {
        List<EnemyAIAction> enemyAIActionList = new List<EnemyAIAction>();
        List<GridPosition> validActionGridPositionList = GetValidActionGridPositionList();

        foreach (var gridPosition in validActionGridPositionList)
        {
            EnemyAIAction enemyAIAction = GetEnemyAIAction(gridPosition);
            enemyAIActionList.Add (enemyAIAction);
        }
        if(enemyAIActionList.Count>0)
        {
            enemyAIActionList.Sort((EnemyAIAction a, EnemyAIAction b)=>b.actionValue-a.actionValue);
            return enemyAIActionList[0];
        }
        else
        {
            return null;
        }
    }
    public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);
}
