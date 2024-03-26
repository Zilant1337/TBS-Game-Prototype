using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }

    public event EventHandler OnSelectedUnitChange;
    public event EventHandler OnSelectedActionChange;
    public event EventHandler<bool> OnBusyChange;
    public event EventHandler OnAPSpent;

    private bool isBusy; 
    [SerializeField] private LayerMask unitLayerMask;
    [SerializeField] private Unit selectedUnit;
    private BaseAction selectedAction;
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
    private void Start()
    {
        SetSelectedUnit(selectedUnit);
    }
    private void Update()
    {
        if (isBusy)
        {
            return;
        }
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (TryHandleUnitSelection())
        {
            return;
        }
        
        HandleSelectedAction();
    }
    private void HandleSelectedAction()
    {
        if (Input.GetMouseButtonDown(0))
        {

            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetMousePosition());

            if (selectedAction.IsValidGridPosition(mouseGridPosition)&& selectedUnit.TryDeductAP(selectedAction))
            {

                SetBusy();
                selectedAction.TakeAction(mouseGridPosition, ClearBusy);
                OnAPSpent?.Invoke(this,EventArgs.Empty);
            }
        }
    }
    private bool TryHandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, Instance.unitLayerMask))
            {
                if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    if(unit!=selectedUnit)
                    {
                        SetSelectedUnit(unit);

                        return true;
                    }
                }
            }
        }
        return false;
    }
    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;

        SetSelectedAction(unit.GetMoveAction());
        
        OnSelectedUnitChange?.Invoke(this, EventArgs.Empty);
        
    }
    public void SetSelectedAction(BaseAction action)
    {
        selectedAction = action;
        OnSelectedActionChange?.Invoke(this, EventArgs.Empty);
    }
    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }
    private void SetBusy()
    {
        isBusy = true;
        OnBusyChange?.Invoke(this, isBusy);
    }
    private void ClearBusy()
    {
        isBusy=false;
        OnBusyChange?.Invoke(this, isBusy);
    }
    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
}
