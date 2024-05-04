using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance { get; private set; }

    [Serializable] public struct GridVisualTypeMaterial
    {
        public GridVisualType gridVisualType;
        public Material material;
    }
    public enum GridVisualType
    {
        White,Blue,LightBlue,Red,LightRed,Yellow,LightYellow,Green,LightGreen
    }
    
    [SerializeField] private Transform gridSystemVisualSinglePrefab;
    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterialList;
    private GridSystemVisualSingle[,] gridSystemVisualSingles;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one GridSystemVisual!!! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        gridSystemVisualSingles = new GridSystemVisualSingle[LevelGrid.Instance.GetWidth(), LevelGrid.Instance.GetHeight()];
        for (int x = 0; x < LevelGrid.Instance.GetWidth();x++)
        {
            for(int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform gridSystemVisualSingleTransform=Instantiate(gridSystemVisualSinglePrefab,LevelGrid.Instance.GetWorldPosition(gridPosition),Quaternion.identity);
                gridSystemVisualSingles[x, z] =gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
            }
        }
        UpdateGridVisual(); 
        UnitActionSystem.Instance.OnSelectedActionChange += UnitActionSystem_OnSelectedActionChange;
        LevelGrid.Instance.OnAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;
        HealthSystem.OnAnyDeath += HealthSystem_OnAnyDeath;
    }
    private void HealthSystem_OnAnyDeath(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }
    private void LevelGrid_OnAnyUnitMovedGridPosition(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }
    private void UnitActionSystem_OnSelectedActionChange(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }
    public void HideAllGridVisuals()
    {
        foreach (var gridSystemVisualSingle in gridSystemVisualSingles)
        {
            gridSystemVisualSingle.Hide();
        }
    }
    private void ShowGridPositionRange(GridPosition gridPosition,int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionList= new List<GridPosition>();
        for (int x = -range; x<= range; x++)
        {
            for (int z = -range; z<= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > range)
                {
                    continue;
                }
                gridPositionList.Add(testGridPosition);
            }
        }
        ShowGridPositionList(gridPositionList, gridVisualType);
    }
    private void ShowGridPositionRangeWithDiagonals(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionList = new List<GridPosition>();
        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                gridPositionList.Add(testGridPosition);
            }
        }
        ShowGridPositionList(gridPositionList, gridVisualType);
    }
    public void ShowGridPositionList(List<GridPosition> gridPositions, GridVisualType gridVisualType)
    {
        foreach (var gridPosition in gridPositions)
        {
            gridSystemVisualSingles[gridPosition.x,gridPosition.z].Show(GetGridVisualTypeMaterial(gridVisualType));
        }
    }
    public void UpdateGridVisual()
    {
        HideAllGridVisuals();
        Unit selectedUnit=UnitActionSystem.Instance.GetSelectedUnit();
        var action = UnitActionSystem.Instance.GetSelectedAction();
        GridVisualType gridVisualType;
        switch (action)
        {
            default:
                gridVisualType = GridVisualType.White;
                break;
            case MoveAction moveAction:
                gridVisualType = GridVisualType.White;
                break;
            case ShootAction shootAction:
                gridVisualType= GridVisualType.Red;
                ShowGridPositionRange(selectedUnit.GetGridPosition(), shootAction.GetMaxShootDistance() ,GridVisualType.LightRed);
                break;
            case SpinAction spinAction:
                gridVisualType = GridVisualType.Green;
                break;
            case SwordAction swordAction:
                gridVisualType = GridVisualType.Red;
                ShowGridPositionRangeWithDiagonals(selectedUnit.GetGridPosition(), swordAction.GetMaxAttackDistance(), GridVisualType.LightRed);
                break;
            case GrenadeAction grenadeAction:
                gridVisualType = GridVisualType.Red;
                break;
            case InteractAction interactAction:
                gridVisualType = GridVisualType.Blue;
                ShowGridPositionRangeWithDiagonals(selectedUnit.GetGridPosition(), interactAction.GetMaxInteractDistance(), GridVisualType.LightBlue);
                break;

        }
        ShowGridPositionList(action.GetValidActionGridPositionList(), gridVisualType);
    }
    public Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
    {
        foreach(var gridVisualTypeMaterial in gridVisualTypeMaterialList)
        {
            if (gridVisualTypeMaterial.gridVisualType == gridVisualType)
            {
                return gridVisualTypeMaterial.material;
            }
        }
        Debug.LogError($"Could not find GridVisualTypeMaterial for GridVisualType {gridVisualType}");
        return null;
    }
}
