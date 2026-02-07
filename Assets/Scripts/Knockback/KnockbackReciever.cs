using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class KnockbackReciever : MonoBehaviour,Iknockbackable
{
    public event Action<KnockbackRequest> OnKnockbackStarted;
   public event Action OnKnockbackEnded;

   [SerializeField] private KnockbackProfile profile;

   [Header("Behavior")]
   [SerializeField] private float settleVelocityThreshold = 0.1f;
   

   private NavMeshAgent agent;
   private Rigidbody rb;
   private Coroutine knockbackRoutine;

   private Vector3 cachedDestination;
   private bool hadPath;
   
   public bool IsInKnockback { get; private set; }
   
   [SerializeField] private AudioSource knockbackAudio;
   [SerializeField] private AudioClip knockbackClip;

   private void Awake()
   {
      agent = GetComponent<NavMeshAgent>(); // get the navmesh of the current agent
      rb = GetComponent<Rigidbody>();
      rb.useGravity = false;
      rb.interpolation = RigidbodyInterpolation.Interpolate;
      rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
      rb.constraints = RigidbodyConstraints.FreezeRotation;
   }

   public void ApplyKnockback(KnockbackRequest request)
   {
      if (profile == null)
      {
         Debug.LogWarning($"{name}: No KnockbackProfile assigned.", this);
         return;
      }
      if (IsInKnockback) // if is already in a knockback dont apply again
         return;
      if (knockbackRoutine != null)
         StopCoroutine(knockbackRoutine);
      if (knockbackAudio && knockbackClip)
      {
         knockbackAudio.PlayOneShot(knockbackClip);
      }
      
      knockbackRoutine = StartCoroutine(DoPhysicalKnockback(request));
      
      Debug.Log("Player knocked!");
   }

   private IEnumerator DoPhysicalKnockback(KnockbackRequest request)
   {
      IsInKnockback = true;
      OnKnockbackStarted?.Invoke(request);
      // so that the agent could resume path after knockback

      agent.isStopped = true; // disable navmesh agent
      agent.updatePosition = false;
      agent.enabled = false;
      
      rb.linearVelocity = Vector3.zero;
      
      Vector3 force = request.Direction * (profile.physicalForceByImpact.Evaluate(request.Distance));
      
      rb.AddForce(force, ForceMode.Impulse);
      
      while (rb.linearVelocity.magnitude > settleVelocityThreshold)
         yield return null;

      float stunDuration = profile.maxStun;
      

      yield return new WaitForSeconds(stunDuration);
      
      agent.enabled = true;
      agent.Warp(transform.position);
      
      Debug.Log($"Stunnged for: {stunDuration}");
      agent.updatePosition = true;
      agent.isStopped = false;
      IsInKnockback = false;
      knockbackRoutine = null;
      OnKnockbackEnded?.Invoke();
   }
}

