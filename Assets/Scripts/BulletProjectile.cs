using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private Vector3 targetPosition;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform bulletHitVFXPrefab;
    public void Setup(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
        Debug.Log($"Target: {targetPosition}");
    }
    private void Update()
    {
        Vector3 moveDirection= (targetPosition - transform.position).normalized;

        float distanceBeforeMoving = Vector3.Distance(transform.position, targetPosition);
        float moveSpeed = 200f;
        transform.position += moveDirection * Time.deltaTime * moveSpeed;
        float distanceAfterMoving = Vector3.Distance(transform.position, targetPosition);

        if (distanceBeforeMoving < distanceAfterMoving)
        {
            transform.position = targetPosition;
            trailRenderer.transform.parent = null;
            Destroy(gameObject);
            Instantiate(bulletHitVFXPrefab, targetPosition, Quaternion.identity);
        }

        if (Vector3.Distance(transform.position, targetPosition) < .1f)
        {
            Destroy(gameObject);
        }
    }
}