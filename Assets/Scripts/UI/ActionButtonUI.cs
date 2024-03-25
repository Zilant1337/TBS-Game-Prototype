using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMeshPro;
    [SerializeField] Button button;
    [SerializeField] GameObject selectedGameObject;
    private BaseAction action;
    public void SetBaseAction(BaseAction action)
    {
        textMeshPro.text= action.GetActionName().ToUpper();
        this.action = action;
        button.onClick.AddListener(()=> { UnitActionSystem.Instance.SetSelectedAction(action); });
    }
    public void UpdateSelectedVisual()
    {
        BaseAction selectedAction=UnitActionSystem.Instance.GetSelectedAction();
        selectedGameObject.SetActive(action==selectedAction);
    }
}
