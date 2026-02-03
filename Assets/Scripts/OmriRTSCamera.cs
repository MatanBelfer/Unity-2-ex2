using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class OmriRTSCamera : MonoBehaviour
{
    [Header("Cinemachine")]
    [SerializeField] private CinemachineFollow follow;

    [Header("Tuning")] 
    [SerializeField] private float moveSpeed;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float minDist;
    [SerializeField] private float maxDist;

    [SerializeField] private Camera mainCamera; // Reference to the main camera for raycasting
    [SerializeField] private LayerMask groundLayerMask = 1; // LayerMask for the ground/terrain layer

    [Header("Following Target")] 
    public Transform followTarget;
    private bool following = false;
    
    
    private void Start()
    {
        InputManager.OnMove += ManualMove;
        InputManager.OnZoom += Zoom;
    }

    private void ManualMove(Vector2 move)
    {
        following = false;
        
        Vector3 delta = new Vector3(move.x, 0f, move.y) * (moveSpeed * Time.deltaTime);
        transform.position += delta;
    }

    private void Zoom(float zoomAmount)
    {
        if (Mathf.Abs(zoomAmount) > 0.001f)
        {
            float dist = follow.FollowOffset.magnitude;
            dist = Mathf.Clamp(dist - zoomAmount * zoomSpeed * Time.deltaTime, minDist, maxDist);
            follow.FollowOffset = follow.FollowOffset.normalized * dist;
        }
    }

    void Update()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        if (following)
        {
            transform.position = Vector3.Lerp(transform.position, followTarget.position, Time.deltaTime * 5f); // Smooth following
        }
    }
}