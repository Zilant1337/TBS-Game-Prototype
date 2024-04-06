using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] MeshRenderer m_MeshRenderer;
    public void Show(Material material)
    {

        m_MeshRenderer.enabled = true;
        m_MeshRenderer.material = material;
    }
    public void Hide()
    {
        m_MeshRenderer.enabled = false;
    }
}
