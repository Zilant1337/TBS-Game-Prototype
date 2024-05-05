using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GrenadeAction : BaseAction
{
    [SerializeField] private Transform grenadeProjectilePrefabTransform;
    [SerializeField] private LayerMask obstaclesLayerMask;
    private int maxThrowDistance = 5;
    private float gridCellExplosionRadius = 2;
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
    }
    public override string GetActionName()
    {
        return "Grenade";
    }
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -maxThrowDistance; x <= maxThrowDistance; x++)
        {
            for (int z = -maxThrowDistance; z <= maxThrowDistance; z++)
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
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxThrowDistance)
                {
                    continue;
                }
                Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                Vector3 shootDirection = (LevelGrid.Instance.GetWorldPosition(testGridPosition) - unitWorldPosition).normalized;
                float unitShoulderHeight = 1.7f;
                if (Physics.Raycast(unitWorldPosition + Vector3.up * unitShoulderHeight, shootDirection,
                    Vector3.Distance(unitWorldPosition, LevelGrid.Instance.GetWorldPosition(testGridPosition)), obstaclesLayerMask))
                {
                    continue;
                }
                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }
    public int GetHitEnemyCount(GridPosition gridPosition)
    {
        int count = 0;
        Collider[] colliderArray = Physics.OverlapSphere(LevelGrid.Instance.GetWorldPosition(gridPosition), 
            gridCellExplosionRadius * LevelGrid.Instance.GetCellSize());

        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent<Unit>(out Unit targetUnit))
            {
                if (!targetUnit.GetIsEnemy() == unit.GetIsEnemy())
                {
                    count++;
                }
            }
        }
        return count;
    }
    public int GetHitFriendlyCount(GridPosition gridPosition)
    {
        int count = 0;
        Collider[] colliderArray = Physics.OverlapSphere(LevelGrid.Instance.GetWorldPosition(gridPosition),
            gridCellExplosionRadius * LevelGrid.Instance.GetCellSize());

        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent<Unit>(out Unit targetUnit))
            {
                if (targetUnit.GetIsEnemy() == unit.GetIsEnemy())
                {
                    count++;
                }
            }
        }
        return count;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        Transform grenadeProjectileTransform = Instantiate(grenadeProjectilePrefabTransform,unit.GetWorldPosition(),Quaternion.identity);
        GrenadeProjectile grenadeProjectile = grenadeProjectileTransform.GetComponent<GrenadeProjectile>();
        grenadeProjectile.Setup(gridPosition, gridCellExplosionRadius, OnGrenadeActionComplete);

        base.ActionStart(onActionComplete);
    }
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int EnemyHurtCount = GetHitEnemyCount(gridPosition);
        int FriendlyHurtCount = GetHitFriendlyCount(gridPosition);
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = EnemyHurtCount*100-FriendlyHurtCount*50,
        };
    }
    private void OnGrenadeActionComplete()
    {
        base.ActionComplete();
    }
}
