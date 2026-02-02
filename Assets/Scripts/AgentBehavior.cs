using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class AgentBehavior : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Vector3 _lastPosition;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (gameObject.transform.position != _lastPosition)
            if (_agent.pathEndPosition == _agent.transform.position && !_agent.pathPending)
                UIManager.Instance.LogMessage(gameObject.name);
        _lastPosition = gameObject.transform.position;
    }
}