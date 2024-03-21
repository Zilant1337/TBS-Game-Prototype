using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    [SerializeField] private Transform gridSystemVisualSinglePrefab;
    private GridSystemVisualSingle[,] gridSystemVisualSingles;
    public static GridSystemVisual Instance { get; private set; }
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
        HideAllGridVisuals();
    }
    private void Update()
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
    public void ShowGridPositionList(List<GridPosition> gridPositions)
    {
        foreach (var gridPosition in gridPositions)
        {
            gridSystemVisualSingles[gridPosition.x,gridPosition.z].Show();
        }
    }
    public void UpdateGridVisual()
    {
        HideAllGridVisuals();
        ShowGridPositionList(UnitActionSystem.Instance.GetSelectedUnit().GetMoveAction().GetValidActionGridPositionList());
    }
}
