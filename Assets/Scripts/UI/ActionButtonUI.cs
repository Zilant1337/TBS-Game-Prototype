using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMeshPro;
    [SerializeField] Button button;
    public void SetBaseAction(BaseAction action)
    {
        textMeshPro.text= action.GetActionName().ToUpper();
    }
}
