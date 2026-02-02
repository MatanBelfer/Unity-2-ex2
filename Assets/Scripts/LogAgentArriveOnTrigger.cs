using System;
using UnityEngine;

public class LogAgentArriveOnTrigger : MonoBehaviour
{
    [SerializeField] [Tooltip("Will be included in logged message when the agent arrives")] private string goalName;
    private const string PLAYER_TAG = "Player";
    private UIManager uiManager;
    
    private void Start()
    {
        uiManager = UIManager.Instance;
        if (!uiManager)
            throw new Exception("UIManager not found");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            uiManager.LogMessage(string.Concat(other.gameObject.name, " reached ", goalName, '!'));
        }
    }
}
