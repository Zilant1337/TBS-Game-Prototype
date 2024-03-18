using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Animator unitAnimator;
    
    private GridPosition gridPosition;
    private MoveAction moveAction;

    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
    }
    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(this.transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition,this);
        moveAction.Move(gridPosition);
    }
    private void Update()
    {
        
        var newGridPosition = LevelGrid.Instance.GetGridPosition(this.transform.position);
        if (newGridPosition != gridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(this,gridPosition,newGridPosition);
            gridPosition = newGridPosition; 
        }
    }
    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }
    public MoveAction GetMoveAction()
    {
        return moveAction;
    }    
    public override string ToString()
    {
        return this.name;
    }
    
}
