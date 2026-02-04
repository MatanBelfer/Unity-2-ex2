using System;
using UnityEngine;
using UnityEngine.AI;

public class AnimateAgentMoving : MonoBehaviour
{
    [SerializeField, HideInInspector] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private string movingParameter;
    [SerializeField] private float moveSpeedThreshold;
    
    private void OnValidate()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool(movingParameter, agent.velocity.sqrMagnitude > moveSpeedThreshold);
    }
}
