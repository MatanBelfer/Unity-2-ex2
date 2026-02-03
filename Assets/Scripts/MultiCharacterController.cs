using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Input = UnityEngine.Windows.Input;

public class MultiCharacterController : MonoBehaviour
{
    [SerializeField] private CharacterComponents[] characters;
    [SerializeField, HideInInspector] private NavMeshAgent[] agentsToControl;
    public Func<CharacterComponents> GetCurrentCharacter;
    private NavMeshAgent currentAgent;

    private void OnValidate()
    {
        agentsToControl = characters.Select(c => c.navMeshAgent).ToArray();
    }

    private void Awake()
    {
        ChangeCharacter(0);
    }

    private void Start()
    {
        InputManager.OnSelectCharacter += number => ChangeCharacter(number - 1);
    }

    private void ChangeCharacter(int agentIndex)
    {
        if (agentIndex >= agentsToControl.Length) return;
        currentAgent = agentsToControl[agentIndex];
        GetCurrentCharacter = () => characters[agentIndex];
        
        print("controlling " + currentAgent.gameObject.name);
    }
}
