using System;

using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    public static event EventHandler OnAnyGrenadeExploded;

    private Vector3 targetPosition;
    private Action onGrenadeActionComplete;
    private float totalDistance;
    private Vector3 posXZ;

    private float moveSpeed= 15f;
    private float reachedTargetDistance =0.2f;
    private int damage=40;
    private float gridCellExplosionRadius;

    [SerializeField] private Transform grenadeExplosionVFXPrefabTransform;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private AnimationCurve arcYAnimationCurve;

    

    private void Update()
    {
        Vector3 moveDirection=(targetPosition - posXZ).normalized;

        posXZ += moveDirection*moveSpeed*Time.deltaTime;

        float distance= Vector3.Distance(posXZ, targetPosition);
        float distanceNormalised = 1 - distance/totalDistance;

        float maxHeight = totalDistance/3f;
        float posY=arcYAnimationCurve.Evaluate(distanceNormalised)*maxHeight;

        transform.position = new Vector3(posXZ.x,posY,posXZ.z);

        if (Vector3.Distance(posXZ, targetPosition) < reachedTargetDistance)
        {
            Collider[] colliderArray=Physics.OverlapSphere(targetPosition, gridCellExplosionRadius * LevelGrid.Instance.GetCellSize());

            foreach (Collider collider in colliderArray)
            {
                if(collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    targetUnit.Damage(damage);
                }
                if(collider.TryGetComponent<DestructableCrate>(out DestructableCrate destructableCrate))
                {
                    destructableCrate.Damage();
                }
            }
            OnAnyGrenadeExploded?.Invoke(this,EventArgs.Empty);
            trailRenderer.transform.parent = null;
            Instantiate(grenadeExplosionVFXPrefabTransform,targetPosition,Quaternion.Euler(-90,0,0));
            Destroy(gameObject);
            onGrenadeActionComplete();
        }
    }
    public void Setup(GridPosition targetGridPosition,float gridCellExplosionRadius, Action onGrenadeActionComplete)
    {
        this.gridCellExplosionRadius = gridCellExplosionRadius;
        this.onGrenadeActionComplete = onGrenadeActionComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);

        posXZ = transform.position;
        posXZ.y = 0;
        totalDistance = Vector3.Distance(posXZ, targetPosition);
    }
}
