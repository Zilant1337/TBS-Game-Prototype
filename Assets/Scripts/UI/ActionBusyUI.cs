using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{
    private void Start()
    {
        UnitActionSystem.Instance.OnBusyChange += UnitActionSystem_OnBusyChange;
        gameObject.SetActive(false);
    }
    public void UnitActionSystem_OnBusyChange(object sender, bool isBusy)
    {
        UpdateEnable(isBusy);
    }
    private void UpdateEnable(bool isBusy)
    {
        gameObject.SetActive(isBusy);
    }
}
