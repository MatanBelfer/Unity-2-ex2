using System;
using UnityEngine;

public class ResetZone : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private void Awake()
    {
        if (!gameManager)
            gameManager = FindObjectOfType<GameManager>();
        if (!gameManager)
            throw new Exception("GameManager not found");
    }

    private void OnTriggerEnter(Collider other)
    {
        CharacterComponents character = other.GetComponentInParent<CharacterComponents>();
        if (character)
            gameManager.NotifyCharacterEnterResetZone(character);
    }

    private void OnTriggerExit(Collider other)
    {
        CharacterComponents character = other.GetComponentInParent<CharacterComponents>();
        if (character)
            gameManager.NotifyCharacterExitResetZone(character);
    }
}
