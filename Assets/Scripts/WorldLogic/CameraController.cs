using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float zoomAmount;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private Vector3 targetFollowOffset;
    [SerializeField] private CinemachineTransposer cinemachineTransposer;

    private const float ZOOM_UPPER_LIMIT = 12f;
    private const float ZOOM_LOWER_LIMIT = 3f;

    private void Start()
    {
        moveSpeed =20f;
        rotationSpeed = 150f;
        zoomAmount = 1.5f;
        zoomSpeed = 7f;
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;
    }
    void Update()
    {
        HandleCameraMovement();
        HandleCameraRotation();
        HandleZoom();
    }
    private void HandleCameraMovement()
    {
        Vector3 inputMoveDirection = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDirection += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDirection += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDirection += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDirection += new Vector3(1, 0, 0);
        }

        var moveVector = transform.forward * inputMoveDirection.z + transform.right * inputMoveDirection.x;
        transform.position += moveVector * Time.deltaTime * moveSpeed;
    }
    private void HandleCameraRotation()
    {
        Vector3 rotationVector = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.Q))
        {
            rotationVector += new Vector3(0, -1, 0);
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotationVector += new Vector3(0, 1, 0);
        }

        transform.eulerAngles += rotationVector * Time.deltaTime * rotationSpeed;
    }
    private void HandleZoom() 
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            targetFollowOffset.y -= Input.mouseScrollDelta.y * zoomAmount;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            targetFollowOffset.y -= Input.mouseScrollDelta.y * zoomAmount;
        }

        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, ZOOM_LOWER_LIMIT, ZOOM_UPPER_LIMIT);
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * zoomSpeed);

    }
}
