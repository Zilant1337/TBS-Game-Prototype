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
    private const float TIMER_DEFAULT_VALUE=0.5f;
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
        foreach(Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            if(TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete)) 
                return true;
        }
        return false;
    }
    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete)
    {
        EnemyAIAction bestEnemyAIAction = null;
        BaseAction bestBaseAction = null;
        foreach (var baseAction in enemyUnit.GetBaseActionArray())
        {
            if (enemyUnit.HasEnoughAPToAct(baseAction))
            {
                if (bestEnemyAIAction == null)
                {
                    bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                    bestBaseAction = baseAction;
                }
                else
                {
                    EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction();
                    if (testEnemyAIAction != null && testEnemyAIAction.actionValue > bestEnemyAIAction.actionValue)
                    {
                        bestEnemyAIAction = testEnemyAIAction;
                        bestBaseAction = baseAction;
                    }
                }
            }
            else
            {
                continue;
            }
        }
        if (bestEnemyAIAction!=null && enemyUnit.TryDeductAP(bestBaseAction))
        {
            bestBaseAction.TakeAction(bestEnemyAIAction.gridPosition,onEnemyAIActionComplete);
            return true;
        }
        else
        {
            return false;
        }
    }

}
