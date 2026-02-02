using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text logObject;

    public static UIManager Instance { get; private set; }

    void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Print a message to the on screen log
    /// </summary>
    public void LogMessage(string message)
    {
        if (!logObject) throw new Exception("Log object is invalid");
        
        logObject.text = string.Concat(message, "\n", logObject.text);
    }
}