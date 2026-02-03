using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MultiCharacterController : MonoBehaviour
{
    private const string GroundLayerName = "Ground";
    [SerializeField] private CharacterComponents[] characters;
    [SerializeField, HideInInspector] private NavMeshAgent[] agentsToControl;
    private NavMeshAgent currentAgent;
    public UnityEvent<CharacterComponents> OnCharacterChange;

    private void OnValidate()
    {
        agentsToControl = characters.Select(c => c.navMeshAgent).ToArray();
    }

    private void Awake()
    {
        ChangeCharacter(1);
    }

    public void ChangeCharacter(int agentNumber)
    {
        int index = agentNumber - 1;
        if (index >= agentsToControl.Length) return;
        currentAgent = agentsToControl[index];
        OnCharacterChange.Invoke(characters[index]);
        
        print("controlling " + currentAgent.gameObject.name);
    }

    public void MoveCharacter()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.value);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 1f);
        if (!Physics.Raycast(ray, out RaycastHit colliderHit, Mathf.Infinity, LayerMask.GetMask(GroundLayerName)))
        {
            return;
        }

        Vector3 pointOnCollider = colliderHit.point;
        // Vector3 pointBeyondCollider = ray.origin + ray.direction * (colliderHit.distance * 2);
        //
        // if (!NavMesh.Raycast(ray.origin, pointBeyondCollider, out NavMeshHit navMeshHit, NavMesh.AllAreas))
        // {
        //     return;
        // }
        
        // Debug.DrawLine(navMeshHit.position, navMeshHit.position + Vector3.up * .5f, Color.green, .1f);
        currentAgent.SetDestination(pointOnCollider);
    }
}
