using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;
    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChange +=UnitActionSystem_OnSelectedUnitChange;
        CreateUnitActionButtons();
    }
    private void CreateUnitActionButtons()
    {
        foreach(Transform buttonTransform in actionButtonContainerTransform)
        {
            Destroy(buttonTransform.gameObject);
        }
        var selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        foreach(var action in selectedUnit.GetBaseActionArray())
        {
            Transform actionButtonTransform=Instantiate(actionButtonPrefab,actionButtonContainerTransform);
            ActionButtonUI actionButtonUI=actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(action);
        }
    }
    private void UnitActionSystem_OnSelectedUnitChange(object sender, EventArgs e)
    { 
        CreateUnitActionButtons();
    }
}
