using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;
    private List<ActionButtonUI> actionButtonUIList;
    private void Awake()
    {
        actionButtonUIList = new List<ActionButtonUI>();
    }
    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChange +=UnitActionSystem_OnSelectedUnitChange;
        UnitActionSystem.Instance.OnSelectedActionChange +=UnitActionSystem_OnSelectedActionChange;
        CreateUnitActionButtons();
        UpdateSelectedVisual();
    }
    private void CreateUnitActionButtons()
    {
        foreach(Transform buttonTransform in actionButtonContainerTransform)
        {
            Destroy(buttonTransform.gameObject);
        }
        actionButtonUIList.Clear();
        var selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        foreach(var action in selectedUnit.GetBaseActionArray())
        {
            Transform actionButtonTransform=Instantiate(actionButtonPrefab,actionButtonContainerTransform);
            ActionButtonUI actionButtonUI=actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUIList.Add(actionButtonUI);
            actionButtonUI.SetBaseAction(action);
        }
    }
    private void UnitActionSystem_OnSelectedUnitChange(object sender, EventArgs e)
    { 
        CreateUnitActionButtons();
        UpdateSelectedVisual();
    }
    private void UnitActionSystem_OnSelectedActionChange(object sender, EventArgs e)
    {
        UpdateSelectedVisual();
    }
    private void UpdateSelectedVisual()
    {
        foreach (var button in actionButtonUIList)
        {
            button.UpdateSelectedVisual();
        }

    }
}
