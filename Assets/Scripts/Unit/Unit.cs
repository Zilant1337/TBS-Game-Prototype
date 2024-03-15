using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Animator unitAnimator;
    private Vector3 targetPosition;
    private float moveSpeed = 5.0f;
    private float stoppingDistance = 0.01f;
    [SerializeField] private float rotationSpeed = 5f;
    private GridPosition gridPosition;

    private void Awake()
    {
        targetPosition = transform.position;
    }
    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(this.transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition,this);
    }
    private void Update()
    {
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance) {
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);
            transform.position += moveDirection * Time.deltaTime * moveSpeed;
            unitAnimator.SetBool("IsWalking", true);
        }
        else
        {
            unitAnimator.SetBool("IsWalking", false);
        }
        var newGridPosition = LevelGrid.Instance.GetGridPosition(this.transform.position);
        if (newGridPosition != gridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(this,gridPosition,newGridPosition);
            gridPosition = newGridPosition; 
        }
    }
    public void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
    public override string ToString()
    {
        return this.name;
    }
    
}
