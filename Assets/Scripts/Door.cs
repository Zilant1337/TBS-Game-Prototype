using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private bool isOpen;
    private GridPosition gridPosition;
    private Animator animator;
    private bool isActive;
    private float timer;
    private Action onInteractComplete;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetDoorAtGridPosition(gridPosition,this);
        if (isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }
    private void Update()
    {
        if (!isActive) 
        {
            return;
        }
        timer-=Time.deltaTime;
        if (timer <= 0)
        {
            isActive = false;
            onInteractComplete();
        }
    }
    public void Interact(Action onInteractComplete)
    {
        this.onInteractComplete = onInteractComplete;
        isActive = true;
        timer = 0.1f;
        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }
    public void OpenDoor()
    {
        isOpen = true;
        animator.SetBool("IsOpen",isOpen);
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition,true);
    }
    public void CloseDoor()
    {
        isOpen = false;
        animator.SetBool("IsOpen", isOpen);
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, false);
    }
}
