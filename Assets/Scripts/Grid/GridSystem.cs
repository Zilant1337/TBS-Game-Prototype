using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem
{
    private int height;
    private int width;
    private float cellSize;
    
    private GridObject[,] gridObjectArray;
    public GridSystem(int width, int height, float cellSize)
    {
        this.width = width; this.height = height; this.cellSize = cellSize;
        gridObjectArray = new GridObject[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                var gridPosition =new GridPosition(x, z);
                gridObjectArray[x,z]=new GridObject(this,gridPosition);
            }
        }
    }
    public Vector3 GetWorldPosition(GridPosition pos)
    {
        return new Vector3(pos.x,0,pos.z)*cellSize;
    }
    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition(Mathf.RoundToInt(worldPosition.x/cellSize),Mathf.RoundToInt(worldPosition.z/cellSize));
    }
    public void CreateDebugObjects(Transform debugPrefab)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                var gridPosition=new GridPosition(x,z);
                Transform debugTransform=GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition),Quaternion.identity );
                GridDebugObject gridDebugObject=debugTransform.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridObject(GetGridObject(gridPosition));
            }
        }
    }
    public GridObject GetGridObject(GridPosition pos)
    {
        return gridObjectArray[pos.x,pos.z];
    }
    public bool IsValidGridPosition(GridPosition pos)
    {
        return pos.x >= 0 && pos.x < width & pos.z >= 0 && pos.z < height;
    }
    public Tuple<int,int> GetGridSize()
    {
        return new Tuple<int,int> (width,height);
    }
    public int GetWidth()
    {
        return width;
    }
    public int GetHeight()
    {
        return height;
    }
    public float GetCellSize()
    {
        return cellSize;
    }
}
