using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    GridSystem gridSystem;
    void Start()
    {
        gridSystem=new GridSystem(10,10,2);

    }
    private void Update()
    {
        Debug.Log(gridSystem.GetGridPosition(MouseWorld.GetMousePosition()));
    }

}
