using UnityEngine;
using UnityEngine.AI;

public class CharacterComponents : MonoBehaviour
{
    public NavMeshAgent navMeshAgent => _navMeshAgent;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    public Transform cameraFollowTarget => _cameraFollowTarget;
    [SerializeField] private Transform _cameraFollowTarget;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private void Awake()
    {
        if (!_navMeshAgent)
            _navMeshAgent = GetComponent<NavMeshAgent>();

        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    private void OnValidate()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void ResetToStart()
    {
        
        if (_navMeshAgent && _navMeshAgent.enabled)
        {
            
            
            _navMeshAgent.ResetPath();
            _navMeshAgent.Warp(startPosition);
            _navMeshAgent.velocity = Vector3.zero;
        }
        
        transform.SetPositionAndRotation(startPosition, startRotation);
    }
}
