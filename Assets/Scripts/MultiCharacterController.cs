using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Input = UnityEngine.Windows.Input;

public class MultiCharacterController : MonoBehaviour
{
    [SerializeField] private CharacterComponents[] characters;
    [SerializeField, HideInInspector] private NavMeshAgent[] agentsToControl;
    public Func<CharacterComponents> GetCurrentCharacter;
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
        GetCurrentCharacter = () => characters[index];
        OnCharacterChange.Invoke(characters[index]);
        
        print("controlling " + currentAgent.gameObject.name);
    }
}
