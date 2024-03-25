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

            if (selectedAction.IsValidGridPosition(mouseGridPosition))
            {
                SetBusy();
                selectedAction.TakeAction(mouseGridPosition, ClearBusy);

            }
            /*switch (selectedAction)
            {
                case MoveAction moveAction:
                    SetBusy();
                    moveAction.TakeAction(mouseGridPosition, ClearBusy);
                    break;
                case SpinAction spinAction:
                    SetBusy();
                    spinAction.TakeAction(mouseGridPosition, ClearBusy);
                    break;
            }*/

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
