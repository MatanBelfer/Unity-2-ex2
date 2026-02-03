using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class OmriRTSCamera : MonoBehaviour
{
    [Header("Cinemachine")]
    [SerializeField] private CinemachineFollow follow;

    [Header("Moving")] 
    [SerializeField] private float moveSpeed;
    private Vector3 moveDelta;
    [Header("Zooming")]
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float minDist;
    [SerializeField] private float maxDist;

    [Header("Following Target")] 
    [SerializeField] private MultiCharacterController characterController;
    public Transform followTarget;
    private bool following = false;
    
    
    private void Start()
    {
        InputManager.OnMove += SetMoveInput;
        InputManager.OnZoom += Zoom;
        InputManager.OnRecenter += Recenter;
    }

    private void SetMoveInput(Vector2 move)
    {
        following = false;
        
        moveDelta = new Vector3(move.x, 0f, move.y) * (moveSpeed * Time.deltaTime);
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

    private void Update()
    {
        FollowTarget();
        
        transform.position += moveDelta;
    }

    private void FollowTarget()
    {
        if (following)
        {
            transform.position = Vector3.Lerp(transform.position, followTarget.position, Time.deltaTime * 5f); // Smooth following
        }
    }

    private void Recenter()
    {
        following = true;
        followTarget = characterController.GetCurrentCharacter().cameraFollowTarget;
    }
}