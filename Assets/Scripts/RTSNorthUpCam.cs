using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using UnityEngine.AI;

public class RTSNorthUpCam : MonoBehaviour
{
    [Header("Cinemachine")] public CinemachineCamera vcam;

    [Header("Input Actions (New Input System)")]
    public InputActionReference move; // Vector2 (WASD)

    public InputActionReference zoom; // Axis (Mouse scroll y)
    public InputActionReference reCenter; // space 
    public InputActionReference rightClick;
    public InputActionReference selectAgent1;
    public InputActionReference selectAgent2;

    [Header("Tuning")] public float moveSpeed = 20f;
    public float zoomSpeed = 8f;
    public float minDist = 10f;
    public float maxDist = 60f;

    CinemachineFollow follow;

    public enum AgentType
    {
        Agent1,
        Agent2
    }

    public AgentType selectedAgentType;
    [SerializeField] private GameObject cameraTargetAgent1;
    [SerializeField] private GameObject cameraTargetAgent2;
    private GameObject selectedAgentCameraTarget;

    [Header("Nav Mesh Movement")] [SerializeField]
    private NavMeshAgent agent1;

    [SerializeField] private NavMeshAgent agent2;
    private NavMeshAgent selectedAgentNav;

    [SerializeField] private Camera mainCamera; // Reference to the main camera for raycasting
    [SerializeField] private LayerMask groundLayerMask = 1; // LayerMask for the ground/terrain layer

    private bool isRightClickHeld = false;
    private bool isSpaceHeld = false;

    void Awake()
    {
        if (vcam != null)
            follow = vcam.GetComponent<CinemachineFollow>();
        else
            Debug.LogError("RTSNorthUpCam: No Cinemachine Camera found");
    }

    void OnEnable()
    {
        if (move != null)
            move.action.Enable();
        
        if (zoom != null)
            zoom.action.Enable();
        
        if (reCenter != null)
        {
            reCenter.action.Enable();
            reCenter.action.started += OnReCenterStarted;
            reCenter.action.canceled += OnReCenterCanceled;
        }
        
        if (rightClick != null)
        {
            rightClick.action.Enable();
            rightClick.action.started += OnRightClickStarted;
            rightClick.action.canceled += OnRightClickCanceled;
        }
        
        if (selectAgent1 != null)
        {
            selectAgent1.action.Enable();
            selectAgent1.action.performed += OnSelectAgent1;
        }
        
        if (selectAgent2 != null)
        {
            selectAgent2.action.Enable();
            selectAgent2.action.performed += OnSelectAgent2;
        }
    }

    void OnDisable()
    {
        if (move != null)
            move.action.Disable();
        
        if (zoom != null)
            zoom.action.Disable();
        
        if (reCenter != null)
        {
            reCenter.action.started -= OnReCenterStarted;
            reCenter.action.canceled -= OnReCenterCanceled;
            reCenter.action.Disable();
        }
        
        if (rightClick != null)
        {
            rightClick.action.started -= OnRightClickStarted;
            rightClick.action.canceled -= OnRightClickCanceled;
            rightClick.action.Disable();
        }
        
        if (selectAgent1 != null)
        {
            selectAgent1.action.performed -= OnSelectAgent1;
            selectAgent1.action.Disable();
        }
        
        if (selectAgent2 != null)
        {
            selectAgent2.action.performed -= OnSelectAgent2;
            selectAgent2.action.Disable();
        }
    }

    void Update()
    {
        selectedAgentCameraTarget = selectedAgentType == AgentType.Agent1 ? cameraTargetAgent1 : cameraTargetAgent2;
        selectedAgentNav = selectedAgentType == AgentType.Agent1 ? agent1 : agent2;
        
        if (isRightClickHeld)
        {
            SetNavMeshDestination();
        }
        
        if (isSpaceHeld && selectedAgentCameraTarget != null)
        {
            transform.position = Vector3.Lerp(transform.position, selectedAgentCameraTarget.transform.position, Time.deltaTime * 5f); // Smooth following
        }
        
        Vector2 m = move != null ? move.action.ReadValue<Vector2>() : Vector2.zero;
        Vector3 delta = new Vector3(m.x, 0f, m.y) * (moveSpeed * Time.deltaTime);
        transform.position += delta;

        if (follow != null && zoom != null)
        {
            float s = zoom.action.ReadValue<float>();
            if (Mathf.Abs(s) > 0.001f)
            {
                float dist = follow.FollowOffset.magnitude;
                dist = Mathf.Clamp(dist - s * zoomSpeed * Time.deltaTime, minDist, maxDist);
                follow.FollowOffset = follow.FollowOffset.normalized * dist;
            }
        }
    }

    private void OnRightClickStarted(InputAction.CallbackContext context)
    {
        isRightClickHeld = true;
    }

    private void OnRightClickCanceled(InputAction.CallbackContext context)
    {
        isRightClickHeld = false;
    }

    private void OnReCenterStarted(InputAction.CallbackContext context)
    {
        isSpaceHeld = true;
    }

    private void OnReCenterCanceled(InputAction.CallbackContext context)
    {
        isSpaceHeld = false;
    }

    private void OnSelectAgent1(InputAction.CallbackContext context)
    {
        SelectAgent(AgentType.Agent1);
    }

    private void OnSelectAgent2(InputAction.CallbackContext context)
    {
        SelectAgent(AgentType.Agent2);
    }

    private void SelectAgent(AgentType agentType)
    {
        if (selectedAgentType != agentType)
        {
            selectedAgentType = agentType;
            Debug.Log($"Agent {agentType} selected");
        }
    }

    void SetNavMeshDestination()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Camera cameraToUse = mainCamera;
        if (cameraToUse == null)
        {
            cameraToUse = Camera.main; // Fallback to main camera
        }

        if (cameraToUse == null)
        {
            Debug.LogWarning("No camera found for raycasting!");
            return;
        }

        Ray ray = cameraToUse.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayerMask))
        {
            if (selectedAgentNav != null)
            {
                selectedAgentNav.SetDestination(hit.point);
            }
            else
            {
                Debug.LogWarning($"Selected agent does not have a NavMeshAgent component!");
            }
        }
        else
        {
            Debug.Log("Raycast did not hit any ground surface.");
        }
    }
    
}