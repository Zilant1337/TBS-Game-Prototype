using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] MeshRenderer m_MeshRenderer;
    public void Show()
    {
        m_MeshRenderer.GetComponent<MeshRenderer>().enabled = true;
    }
    public void Hide()
    {
        m_MeshRenderer.GetComponent<MeshRenderer>().enabled = false;
    }
}
