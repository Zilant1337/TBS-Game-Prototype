using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoveAction : BaseAction
{
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;

    private List<Vector3>positionWaypoints;
    private int currentPositionIndex;

    private float moveSpeed = 5.0f;
    private float stoppingDistance = 0.01f;
    private float rotationSpeed = 15f;

    [SerializeField] int maxMoveDistance=4;

    // Update is called once per frame
    protected override void Awake()
    {
        base.Awake();
        actionPointCost = 1;
    }
    void Update()
    {
        if (!isActive)
            return;
        Vector3 targetPosition = positionWaypoints[currentPositionIndex];
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);

        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            transform.position += moveDirection * Time.deltaTime * moveSpeed;
        }
        else
        {
            currentPositionIndex++;
            if(currentPositionIndex>=positionWaypoints.Count)
            {
                OnStopMoving?.Invoke(this,EventArgs.Empty);
                base.ActionComplete();
            }
        }

        
    }
    public override void TakeAction(GridPosition targetPosition, Action onMoveComplete)
    {
        List<GridPosition> gridPositionList=Pathfinding.Instance.FindPath(unit.GetGridPosition(),targetPosition,out int pathLength);

        currentPositionIndex = 0;
        positionWaypoints = new List<Vector3>();
        foreach(GridPosition gridPos in gridPositionList)
        {
            positionWaypoints.Add(LevelGrid.Instance.GetWorldPosition(gridPos));
        }
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
                if (!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition))
                {
                    continue;
                }
                if (!Pathfinding.Instance.IsReachableGridPosition(unitGridPosition,testGridPosition))
                {
                    continue; 
                }
                int pathfindingDistanceMultiplier =10;
                if(Pathfinding.Instance.GetPathLength(unitGridPosition, testGridPosition) > maxMoveDistance*pathfindingDistanceMultiplier)
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
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        //Параметр, указывающий на количество противников по которым может пострелять юнит, если перейдёт на заданную клетку
        int targetCountAtGridPosition = unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);
        //Список всех клеток, куда юнит может кинуть гранату
        List<GridPosition> grenadeThrowOptions = unit.GetAction<GrenadeAction>().GetValidActionGridPositionList();
        int maxEnemiesHit =0;
        //Поиск максимального количества врагов, которых юнит сможет задеть броском гранаты
        foreach (GridPosition gridPos in grenadeThrowOptions)
        {
            int enemiesHit= unit.GetAction<GrenadeAction>().GetHitEnemyCount(gridPos);
            if (enemiesHit > maxEnemiesHit)
            {
                maxEnemiesHit= enemiesHit;
            }
        }
        //Расчёт ценности приоритизирует выстрелы выше бросков гранат в угоду игровому балансу
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition * 10+ maxEnemiesHit * 5,
        };
    }
}
