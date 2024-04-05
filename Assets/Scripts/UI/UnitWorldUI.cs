using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionPointsText;
    [SerializeField] private Unit unit;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private TextMeshProUGUI healthBarText;
    private void Start()
    {
        Unit.OnAnyAPChange += Unit_OnAnyAPChange;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;

        UpdateActionPointsText();
        UpdateHealthUI();
    }
    
    private void UpdateActionPointsText()
    {
        actionPointsText.text = $"AP: {unit.GetAP().ToString()}";
    }
    private void UpdateHealthUI()
    {
        healthBarImage.fillAmount=healthSystem.GetHPNormalized();
        healthBarText.text = $"{healthSystem.GetHealth()}/{healthSystem.GetHealthMax()}";
    }
    private void Unit_OnAnyAPChange(object sender, EventArgs e)
    {
        UpdateActionPointsText();
    }
    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        UpdateHealthUI();
    }
}
