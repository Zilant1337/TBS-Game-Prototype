using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableCrate : MonoBehaviour
{
    public static event EventHandler OnAnyCrateDestroyed;
    private GridPosition gridPosition;

    [SerializeField] private Transform CrateDestroyedPrefab;
    private void Start()
    {
        gridPosition=LevelGrid.Instance.GetGridPosition(transform.position);
    }
    public void Damage()
    {
        Transform crateDestroyedTransform = Instantiate(CrateDestroyedPrefab,transform.position,transform.rotation);

        ApplyExplosionToChildren(crateDestroyedTransform,150f,transform.position,10f);
        Destroy(gameObject);

        OnAnyCrateDestroyed?.Invoke(this,EventArgs.Empty);
    }
    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }
    private void ApplyExplosionToChildren(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }
            ApplyExplosionToChildren(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}
