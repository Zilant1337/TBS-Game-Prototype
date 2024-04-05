using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Animator unitAnimator;

    public static event EventHandler OnAnyAPChange;

    [SerializeField] private bool isEnemy;

    private const int ACTION_POINTS_MAX = 2;
    private GridPosition gridPosition;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private BaseAction[] baseActionArray;
    private HealthSystem healthSystem;
    private int actionPoints = ACTION_POINTS_MAX;
    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionArray =GetComponents<BaseAction>();
        healthSystem = GetComponent<HealthSystem>();
    }
    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(this.transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition,this);
        TurnSystem.Instance.OnTurnEnd +=TurnSystem_OnTurnEnd;
        healthSystem.OnDead += HealthSystem_OnDead;

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
    public Vector3 GetWorldPosition()
    {
        return transform.position;
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
    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        Destroy(gameObject);
    }
    private void TurnSystem_OnTurnEnd(object sender, EventArgs e)
    {
        if(!isEnemy&&TurnSystem.Instance.GetIsPlayerTurn()||isEnemy&&!TurnSystem.Instance.GetIsPlayerTurn())
        ResetAP();
        
    }
    public void ResetAP()
    {
        actionPoints = ACTION_POINTS_MAX;
        OnAnyAPChange?.Invoke(this, EventArgs.Empty);
    }
    public bool GetIsEnemy()
    {
        return isEnemy;
    }
    public void SetIsEnemy(bool isEnemy)
    {
        this.isEnemy = isEnemy;
    }
    public void Damage(int damageDealt)
    {
        healthSystem.Damage(damageDealt);
    }
}
