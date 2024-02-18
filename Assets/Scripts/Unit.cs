using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Vector3 targetPosition;
    private float moveSpeed = 5.0f;
    private float stoppingDistance = 0.01f;
    private void Update()
    {
        Vector3 moveDirection= (targetPosition- transform.position).normalized;
        if (Vector3.Distance(transform.position,targetPosition)>stoppingDistance){

            transform.position += moveDirection * Time.deltaTime * moveSpeed;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Move(new Vector3(4, 0, 4));
        }
    }
    private void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;

    }
}
