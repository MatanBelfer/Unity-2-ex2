using UnityEngine;
using UnityEngine.AI;

public class AgentAnimator : MonoBehaviour
{
    private Animator _animator;
    private NavMeshAgent _agent;
    
    private int _waterAreaIndex;
    
    private static readonly int MoveX = Animator.StringToHash("MoveX");
    private static readonly int MoveY = Animator.StringToHash("MoveY");
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int IsSwimming = Animator.StringToHash("IsSwimming");
    private static readonly int Wave = Animator.StringToHash("Wave");


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponentInParent<NavMeshAgent>();
        
        _waterAreaIndex = NavMesh.GetAreaFromName("Water");
    }

    // private void OnEnable()
    // {
    //     if (InputManager.Instance.OnSelectCharacter != null)
    //         InputManager.Instance.OnWave.AddListener(PlayWave);
    // }
    //
    // private void OnDisable()
    // {
    //     if (InputManager.Instance.OnSelectCharacter != null)
    //         InputManager.Instance.OnWave.RemoveListener(PlayWave);
    // }

    private void Update()
    {
        if (!_agent) return;
        
        UpdateMovementParameters();
        UpdateWaterState();
    }

    private void UpdateMovementParameters()
    {
        var velocity = _agent.velocity;
        // Convert to local space relative to the child's transform
        var localVelocity = transform.InverseTransformDirection(velocity);
        
        var speed = _agent.remainingDistance > _agent.stoppingDistance ? velocity.magnitude : 0f;
        
        _animator.SetFloat(MoveX, localVelocity.x, 0.1f, Time.deltaTime);
        _animator.SetFloat(MoveY, localVelocity.z, 0.1f, Time.deltaTime);
        _animator.SetFloat(Speed, speed);
    }

    private void UpdateWaterState()
    {
        if (!NavMesh.SamplePosition(transform.position, out var hit, 0.5f, NavMesh.AllAreas))
            return;
        
        var isInWater = (hit.mask & (1 << _waterAreaIndex)) != 0;
        
        _animator.SetBool(IsSwimming, isInWater);
    }

    public void PlayWave()
    {
        _animator.SetTrigger(Wave);
    }
}