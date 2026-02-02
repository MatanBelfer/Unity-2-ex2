using UnityEngine;

public class KnockbackObstacle : MonoBehaviour
{
   [SerializeField] private KnockbackProfile profile;
   [SerializeField] private LayerMask affectedLayers = ~0;
   
   [Header("RigidBody")]
   [SerializeField] private Rigidbody rb;
   
   private void Awake()
   {
      if (rb == null)
         rb = GetComponentInParent<Rigidbody>();
   }
   
   private void OnCollisionEnter(Collision collision)
   {
      TryApply(collision);
      Debug.Log("Player knocked!");
   }
   
   private void OnCollisionStay(Collision collision)
   {
      TryApply(collision);
   }
   
   private void TryApply(Collision collision)
   {
      if (profile == null) return;

      int otherLayerMask = 1 << collision.gameObject.layer;
      if ((affectedLayers.value & otherLayerMask) == 0)
         return;

      if (!collision.gameObject.TryGetComponent<Iknockbackable>(out var knockbackable))
         return;

     
      ContactPoint contact = collision.GetContact(0);
      Vector3 n = contact.normal.normalized;

     
      Vector3 wallVel = Vector3.zero;
      if (rb != null)
         wallVel = rb.GetPointVelocity(contact.point);
      
      Vector3 otherVel = Vector3.zero;
      if (collision.gameObject.TryGetComponent<UnityEngine.AI.NavMeshAgent>(out var agent))
         otherVel = agent.velocity;

      Vector3 relVel = wallVel - otherVel;

      float impactSpeed = Vector3.Dot(relVel, n);

      if (impactSpeed < profile.minImpactSpeed)
         return;

      float distance = profile.distanceByImpactSpeed.Evaluate(impactSpeed);
      float duration = profile.durationByImpactSpeed.Evaluate(impactSpeed);

      distance = Mathf.Min(distance, profile.maxDistance);
      duration = Mathf.Min(duration, profile.maxDuration);
      
      Vector3 dir = n;

      var request = new KnockbackRequest(
         direction: dir,
         distance: distance,
         duration: duration,
         sourcePoint: contact.point,
         source: gameObject
      );

      knockbackable.ApplyKnockback(request);
   }
}
