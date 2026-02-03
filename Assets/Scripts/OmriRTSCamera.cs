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
    public Transform followTarget;
    private bool following = false;
    
    public void SetMoveInput(Vector2 move)
    {
        following = false;
        
        moveDelta = new Vector3(move.x, 0f, move.y) * (moveSpeed * Time.deltaTime);
    }

    public void Zoom(float zoomAmount)
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

    public void Recenter()
    {
        following = true;
    }
    
    public void ChangeFollowingCharacter(CharacterComponents character)
    {
        followTarget = character.cameraFollowTarget;
    }
}