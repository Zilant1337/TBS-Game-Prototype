using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }
    public event EventHandler OnSelectedUnitChange;
    private bool isBusy; 
    [SerializeField] private LayerMask unitLayerMask;
    [SerializeField] private Unit selectedUnit;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one UnitActionSystem!!! "+ transform+" - "+Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
    }
    private void Update()
    {
        if (isBusy)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0)) 
        {
            if (TryHandleUnitSelection())
            {
                return;
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            SetBusy();
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetMousePosition());
            if (selectedUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition))
                selectedUnit.GetMoveAction().Move(mouseGridPosition,ClearBusy);
           
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            SetBusy();
            selectedUnit.GetSpinAction().Spin(ClearBusy);
        }
    }
    private bool TryHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, Instance.unitLayerMask))
        {
            if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit)) 
            {
                SetSelectedUnit(unit);

                return true;
            }
        }
        return false;
    }
    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        OnSelectedUnitChange?.Invoke(this, EventArgs.Empty);
        
    }
    private void SetBusy()
    {
        isBusy = true;
    }
    private void ClearBusy()
    {
        isBusy=false;
    }
    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
}
