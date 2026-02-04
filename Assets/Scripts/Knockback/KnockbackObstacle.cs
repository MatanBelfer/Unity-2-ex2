using UnityEngine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KnockbackObstacle : MonoBehaviour
{
    [Header("Knockback")] [SerializeField] private KnockbackProfile profile;
    [SerializeField] private LayerMask affectedLayers = ~0;

    [Header("Physics")] [SerializeField] private Rigidbody rb;

    [Header("Cooldown")] [SerializeField] private float perTargetCooldown = 1f;

    [SerializeField] private float distanceMultiplier = 4f;


    private readonly Dictionary<int, float> nextAllowedTime = new();

    private Collider triggerCollider;

    private void Awake()
    {
        triggerCollider = GetComponent<Collider>();

        if (!triggerCollider)
        {
            Debug.LogError("KnockbackTriggerObstacle requires a Collider", this);
        }

        if (!triggerCollider.isTrigger)
        {
            Debug.LogError("Collider must be set to IsTrigger = true", this);
        }

        if (rb == null)
            rb = GetComponentInParent<Rigidbody>();
    }

    private void OnTriggerStay(Collider other)
    {
        int otherLayerMask = 1 << other.gameObject.layer;
        if ((affectedLayers & otherLayerMask) == 0)
            return;


        var knockbackable = other.GetComponentInParent<Iknockbackable>();
        if (knockbackable == null || knockbackable.IsInKnockback)
            return;

        if (profile == null)
            return;


        int id = knockbackable.GetHashCode();
        if (nextAllowedTime.TryGetValue(id, out float allowedTime))
        {
            if (Time.time < allowedTime)
                return;
        }

        nextAllowedTime[id] = Time.time + perTargetCooldown;


        Vector3 closest = triggerCollider.ClosestPoint(other.transform.position);


        Vector3 dir = other.transform.position - closest;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.0001f)
            return;

        dir.Normalize();


        Vector3 wallVel = rb ? rb.GetPointVelocity(closest) : Vector3.zero;

        Vector3 otherVel = Vector3.zero;
        if (other.GetComponentInParent<NavMeshAgent>() is NavMeshAgent agent)
            otherVel = agent.velocity;

        float impactSpeed = Vector3.Dot(wallVel - otherVel, dir);
        if (impactSpeed < profile.minImpactSpeed)
            return;

        float distance = Mathf.Min(
            profile.distanceByImpactSpeed.Evaluate(impactSpeed) * distanceMultiplier,
            profile.maxDistance
        );


        float duration = Mathf.Min(
            profile.durationByImpactSpeed.Evaluate(impactSpeed),
            profile.maxDuration
        );

        knockbackable.ApplyKnockback(
            new KnockbackRequest(
                dir,
                distance,
                duration,
                closest,
                gameObject
            )


        );
        Debug.Log(
            $"impactSpeed={impactSpeed:F2}, dist={distance:F2}, dur={duration:F2}",
            this);
        Debug.Log("player knocked!");
        Debug.Log(
            $"Trigger hit by {other.name} on layer {LayerMask.LayerToName(other.gameObject.layer)}",
            this
        );
    }
}
