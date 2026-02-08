using System;
using UnityEngine;

public class ResetZone : MonoBehaviour
{






 private void OnTriggerEnter(Collider other)
    {
        CharacterComponents character = other.GetComponentInParent<CharacterComponents>();
        if (character)
            GameManager.Instance.NotifyCharacterEnterResetZone(character);
    }

    private void OnTriggerExit(Collider other)
    {
        CharacterComponents character = other.GetComponentInParent<CharacterComponents>();
        if (character)
            GameManager.Instance.NotifyCharacterExitResetZone(character);
    }
}