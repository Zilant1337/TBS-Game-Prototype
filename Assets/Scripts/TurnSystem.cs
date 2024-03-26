using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    private int currentTurn=1;
    public static TurnSystem Instance;

    public event EventHandler OnTurnEnd;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one TurnSystem!!! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void NextTurn()
    {
        currentTurn++;
        OnTurnEnd?.Invoke(this, EventArgs.Empty);
    }
    public int GetCurrentTurn()
    {
        return currentTurn;
    }
}
