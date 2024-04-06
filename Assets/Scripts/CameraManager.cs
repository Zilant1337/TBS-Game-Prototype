using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManagerScript : MonoBehaviour
{
    [SerializeField] private GameObject actionCameraGameObject;

    private void Start()
    {
        BaseAction.OnActionStart += BaseAction_OnActionStart;
        BaseAction.OnActionEnd += BaseAction_OnActionEnd;
    }

    private void BaseAction_OnActionEnd(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                
                HideActionCamera();
                break;

        }
    }

    private void BaseAction_OnActionStart(object sender,EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                Unit shooterUnit = shootAction.GetUnit();
                Unit targetUnit = shootAction.GetTargetUnit();

                Vector3 cameraCharacterHeight = Vector3.up * 1.7f;

                Vector3 shootingDirection = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;

                float shoulderOffsetAmount = 0.5f;
                Vector3 shoulderOffset = Quaternion.Euler(0, 90, 0) * shootingDirection * shoulderOffsetAmount;

                Vector3 actionCameraPosition=
                    shooterUnit.GetWorldPosition() + cameraCharacterHeight + shoulderOffset + (shootingDirection * -1);
                actionCameraGameObject.transform.position = actionCameraPosition;
                actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition()+cameraCharacterHeight);
                ShowActionCamera();
                break;
        
        }
    }
    private void ShowActionCamera()
    {
        actionCameraGameObject.SetActive(true);
    }
    private void HideActionCamera()
    {
        actionCameraGameObject.SetActive(false);
    }
}
