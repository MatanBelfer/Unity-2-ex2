using System;
using UnityEngine;
using UnityEngine.AI;

public class CharacterComponents : MonoBehaviour
{
    public NavMeshAgent navMeshAgent => _navMeshAgent;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    public Transform cameraFollowTarget => _cameraFollowTarget;
    [SerializeField] private Transform _cameraFollowTarget;

    private void OnValidate()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }
}
