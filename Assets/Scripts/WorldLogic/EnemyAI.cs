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
    private const float TIMER_DEFAULT_VALUE=1f;
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
        //Проходится по всем юнитам, подконтрольным противнику
        foreach(Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            //Проверяется может ли юнит может выполнить действие.
            if(TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete)) 
                return true;
        }
        return false;
    }
    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete)
    {
        //Задаются параметры (Действие, его ценность и клетка, при применении действия на которой она достигается), по которым мы выберем действие с наибольшей ценностью
        EnemyAIAction bestEnemyAIAction = null;
        BaseAction bestBaseAction = null;
        //В цикле проходится по всем доступным данному юниту действиям
        foreach (var baseAction in enemyUnit.GetBaseActionArray())
        {
            //Проверяется достаточно ли у юнита очков действия
            if (enemyUnit.HasEnoughAPToAct(baseAction))
            {
                //Если ещё нет значений в параметрах, они записываются
                if (bestEnemyAIAction == null)
                {
                    bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                    bestBaseAction = baseAction;
                }
                //Если же параметры уже имеют значения, то в случае высшей чем в записанных параметрах ценности действия, перезаписываем параметры
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
        //Если есть действие, которое мы можем совершить, вычитаем у юнита нужное количество очков действия и выполняем его на клетку, где ценность действия максимальна
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
