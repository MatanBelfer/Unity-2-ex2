using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject spawnPoint;
    
    [SerializeField] private MultiCharacterController characterController;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private string resetPromptMessage = "Press R to reset the selected character.";
    

    private readonly HashSet<CharacterComponents> charactersInResetZone = new();
    private CharacterComponents selectedCharacter;
    private bool promptShown;


    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        inputManager = InputManager.Instance;
        DontDestroyOnLoad(gameObject);
        
    }

    private void OnEnable()
    {
        characterController.OnCharacterChange.AddListener(HandleCharacterChange);
        inputManager.OnReset.AddListener(HandleReset);
    }

    private void OnDisable()
    {
        if (characterController)
            characterController.OnCharacterChange.RemoveListener(HandleCharacterChange);
        if (inputManager)
            inputManager.OnReset.RemoveListener(HandleReset);
    }

    private void Start()
    {
        selectedCharacter = characterController.CurrentCharacter;
        UpdateResetPrompt();
    }

    public void NotifyCharacterEnterResetZone(CharacterComponents character)
    {
        if (!character)
            return;

        charactersInResetZone.Add(character);
        UpdateResetPrompt();
    }

    public void NotifyCharacterExitResetZone(CharacterComponents character)
    {
        if (!character)
            return;

        charactersInResetZone.Remove(character);
        UpdateResetPrompt();
    }

    private void HandleCharacterChange(CharacterComponents character)
    {
        selectedCharacter = character;
        UpdateResetPrompt();
    }

    private void HandleReset()
    {
        if (!selectedCharacter)
            return;

        if (!charactersInResetZone.Contains(selectedCharacter))
            return;

        if (SurfaceTypeManager.Instance.GetSurfaceNameByMask(selectedCharacter.navMeshAgent.areaMask) == "Spectator")
        {
            characterController.Respawn();
        }
        else
        {
            selectedCharacter.ResetToStart();    
        }
        UIManager.Instance.LogMessage($"{selectedCharacter.gameObject.name} reset to start.");
    }

    private void UpdateResetPrompt()
    {
        if (selectedCharacter && charactersInResetZone.Contains(selectedCharacter))
        {
            if (!promptShown)
            {
                UIManager.Instance.LogMessage(resetPromptMessage);
                promptShown = true;
            }
        }
        else
        {
            promptShown = false;
        }
    }
}