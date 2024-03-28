using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI turnCounterText;
    [SerializeField] Button endTurnButton;
    [SerializeField] GameObject enemyTurnVisual;
    private void Start()
    {
        endTurnButton.onClick.AddListener(() => { TurnSystem.Instance.NextTurn(); });
        Unit.OnAnyAPChange += Unit_OnAnyAPChange;
        TurnSystem.Instance.OnTurnEnd+=TurnSystem_OnTurnEnd;
        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisual(); 
    }
    private void TurnSystem_OnTurnEnd(object sender, EventArgs e)
    {
        UpdateTurnText();
        UpdateEnemyTurnVisual();
    }
    private void UpdateEnemyTurnVisual()
    {
        enemyTurnVisual.SetActive(!TurnSystem.Instance.GetIsPlayerTurn());
        Debug.Log(TurnSystem.Instance.GetIsPlayerTurn());
        UpdateEndTurnButtonVisual();
    }
    private void Unit_OnAnyAPChange(object sender, EventArgs e)
    {
        UpdateTurnText();
    }
    private void UpdateTurnText()
    {
        turnCounterText.text =$"Turn {TurnSystem.Instance.GetCurrentTurn()}";
    }
    private void UpdateEndTurnButtonVisual()
    {
        endTurnButton.gameObject.SetActive(TurnSystem.Instance.GetIsPlayerTurn());
    }
}
