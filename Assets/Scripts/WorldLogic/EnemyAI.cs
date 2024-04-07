using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy
    }
    private State currentState; 
    private float timer;
    private const float TIMER_DEFAULT_VALUE=5;
    // Update is called once per frame
    private void Awake()
    {
        currentState = State.WaitingForEnemyTurn;
    }
    private void Start()
    {
        TurnSystem.Instance.OnTurnEnd +=TurnSystem_OnTurnEnd;
    }
    void Update()
    {
        if (TurnSystem.Instance.GetIsPlayerTurn())
        {
            return;
        }
        switch (currentState)
        {
            case State.WaitingForEnemyTurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    currentState=State.Busy;
                    if (TryTakeEnemyAIAction(SetStateTakingTurn))
                    {
                        currentState = State.Busy;
                    }
                    else
                    {
                        TurnSystem.Instance.NextTurn();
                        currentState = State.WaitingForEnemyTurn;
                    }
                }
                break;
            case State.Busy:
                break;
        }
        
    }
    private void SetStateTakingTurn()
    {
        timer= 0.5f;
        currentState = State.TakingTurn;
    }
    private void TurnSystem_OnTurnEnd(object sender, EventArgs e)
    {
        if (!TurnSystem.Instance.GetIsPlayerTurn())
        {
            currentState=State.TakingTurn;
            timer = TIMER_DEFAULT_VALUE;
        }
    }
    private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
    {
        Debug.Log("Take Enemy AI Action");
        foreach(Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            if(TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete)) 
                return true;
        }
        return false;
    }
    private bool TryTakeEnemyAIAction(Unit enemyUnit,Action onEnemyAIActionComplete)
    {
        SpinAction spinAction = enemyUnit.GetSpinAction();
        while (enemyUnit.GetAP() > 0)
        {
            GridPosition actionGridPosition = enemyUnit.GetGridPosition();

            if (spinAction.IsValidGridPosition(actionGridPosition) && enemyUnit.TryDeductAP(spinAction))
            {
                Debug.Log($"You spin {enemyUnit} right round, baby, right round!");
                spinAction.TakeAction(actionGridPosition, onEnemyAIActionComplete);
                return true;
            }
            else
            {
                return false;
            }
            
        }
        return false;  
    }

}
