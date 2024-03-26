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
    private void Start()
    {
        endTurnButton.onClick.AddListener(() => { TurnSystem.Instance.NextTurn(); });
        Unit.OnAnyAPChange += Unit_OnAnyAPChange;
        UpdateTurnText();
    }
    private void Unit_OnAnyAPChange(object sender, EventArgs e)
    {
        UpdateTurnText();
    }
    private void UpdateTurnText()
    {
        turnCounterText.text =$"Turn {TurnSystem.Instance.GetCurrentTurn()}";
    }
}
