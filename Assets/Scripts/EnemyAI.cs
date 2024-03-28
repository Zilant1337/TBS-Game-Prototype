using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private float timer;
    private const float TIMER_DEFAULT_VALUE=5;
    // Update is called once per frame
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
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            TurnSystem.Instance.NextTurn();
        }
    }
    private void TurnSystem_OnTurnEnd(object sender, EventArgs e)
    {
        timer = TIMER_DEFAULT_VALUE;
    }
}
