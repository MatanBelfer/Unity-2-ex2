using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class KnockbackReceiver : MonoBehaviour,Iknockbackable
{
   public event Action<KnockbackRequest> OnKnockbackStarted;
   public event Action OnKnockbackEnded;

   [SerializeField] private KnockbackProfile profile;

   [Header("Behavior")] [SerializeField] private bool interruptKnockback;
   [SerializeField] private float navmeshRadius = 0.5f;

   private NavMeshAgent agent;
   private Coroutine knockbackRoutine;

   private Vector3 cachedDestination;
   private bool hadPath;
   
   public bool IsInKnockback { get; private set; }

   private void Awake()
   {
      agent = GetComponent<NavMeshAgent>(); // get the navmesh of the current agent
   }

   public void ApplyKnockback(KnockbackRequest request)
   {
      if (profile == null)
      {
         Debug.LogWarning($"{name}: No KnockbackProfile assigned.", this);
         return;
      }
      if (!interruptKnockback && IsInKnockback) // if is already in a knockback dont apply again
         return;
      if (knockbackRoutine != null)
         StopCoroutine(knockbackRoutine);
      
      knockbackRoutine = StartCoroutine(DoKnockback(request));
      
      Debug.Log("Player knocked!");
   }

   private IEnumerator DoKnockback(KnockbackRequest request)
   {
      IsInKnockback = true;
      OnKnockbackStarted?.Invoke(request);
      
      // so that the agent could resume path after knockback
      hadPath = agent.hasPath;
      cachedDestination = agent.destination;

      agent.isStopped = true;
      
      float duration = Mathf.Max(0.01f, request.Duration);
      float distance = Mathf.Max(0f, request.Distance);
      
      Vector3 dir = request.Direction;
      dir.y = 0f;
      if (dir.sqrMagnitude < 0.0001f)
         dir = -transform.forward;
      dir.Normalize();
      
      float elapsed = 0f;
      float movedSoFar = 0f;

      while (elapsed < duration)
      {
         elapsed += Time.deltaTime;
         float t = Mathf.Clamp01(elapsed / duration);

         float eased = profile.displacementEase.Evaluate(t);
         float targetMoved = eased * distance;

         float deltaMove = targetMoved - movedSoFar;
         movedSoFar = targetMoved;

         Vector3 delta = dir * deltaMove;
         
         Vector3 desiredPos = transform.position + delta;

         if (NavMesh.SamplePosition(desiredPos, out var hit, navmeshRadius, agent.areaMask))
         {
            Vector3 correctedDelta = hit.position - transform.position;
            agent.Move(correctedDelta);
         }
         else
         {
            agent.Move(delta);
         }

         yield return null;
      }

      agent.isStopped = false;
      if (hadPath)
         agent.SetDestination(cachedDestination);
      IsInKnockback = false;
      knockbackRoutine = null;
      Debug.Log("Player knocked!");
      OnKnockbackEnded?.Invoke();
   }
}

