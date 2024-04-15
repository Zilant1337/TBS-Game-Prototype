using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject 
{
    private GridPosition gridPosition;
    private GridSystem<GridObject> gridSystem;
    private List<Unit> unitList;
    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        unitList = new List<Unit>();
    }
    public override string ToString()
    {
        string unitNames="";
        foreach(Unit i in unitList)
        {
            unitNames+=i.ToString()+"\n";
        }
        return gridPosition.ToString()+"\n"+unitNames;
    }
    public List<Unit> GetUnitList()
    {
        return unitList;
    }
    public void AddUnit(Unit unit)
    {
        unitList.Add(unit);
    }
    public void RemoveUnit(Unit unit)
    {
        unitList.Remove(unit);
    }
    public bool HasAnyUnit() 
    {
        return unitList.Count > 0;
    }
    public Unit GetFirstUnit()
    {
        if(HasAnyUnit())
            return unitList[0];
        else return null;
    }
}
