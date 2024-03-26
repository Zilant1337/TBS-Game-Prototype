using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Animator unitAnimator;

    public static event EventHandler OnAnyAPChange;

    private const int ACTION_POINTS_MAX = 2;
    private GridPosition gridPosition;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private BaseAction[] baseActionArray;
    private int actionPoints = ACTION_POINTS_MAX;
    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionArray =GetComponents<BaseAction>();
    }
    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(this.transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition,this);
        TurnSystem.Instance.OnTurnEnd +=TurnSystem_OnTurnEnd;

    }
    private void Update()
    {
        
        var newGridPosition = LevelGrid.Instance.GetGridPosition(this.transform.position);
        if (newGridPosition != gridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(this,gridPosition,newGridPosition);
            gridPosition = newGridPosition; 
        }
    }
    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }
    public MoveAction GetMoveAction()
    {
        return moveAction;
    }    
    public override string ToString()
    {
        return this.name;
    }
    public SpinAction GetSpinAction()
    {
        return spinAction;
    }
    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }
    public bool HasEnoughAPToAct(BaseAction baseAction)
    {
        return (actionPoints - baseAction.GetActionPointCost() >= 0);
    }
    private void DeductAP(int amount)
    {
        actionPoints-=amount;
        OnAnyAPChange?.Invoke(this,EventArgs.Empty);
    }
    public bool TryDeductAP(BaseAction baseAction)
    {
        if (HasEnoughAPToAct(baseAction))
        {
            DeductAP(baseAction.GetActionPointCost());
            return true;
        }
        return false;
    }
    public int GetAP()
    {
        return actionPoints;
    }
    private void TurnSystem_OnTurnEnd(object sender, EventArgs e)
    {
        ResetAP();
    }
    public void ResetAP()
    {
        actionPoints = ACTION_POINTS_MAX;
        OnAnyAPChange?.Invoke(this, EventArgs.Empty);
    }
}
