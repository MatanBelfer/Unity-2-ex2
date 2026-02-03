using System;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    
    private InputSystem_Actions actions;
    public UnityEvent OnInteract;
    public UnityEvent<Vector2> OnMove;
    public UnityEvent<float> OnZoom;
    public UnityEvent<int> OnSelectCharacter;
    public UnityEvent OnRecenter;
    
    private void Awake()
    {
        Instance = this;
        
        actions = new InputSystem_Actions();
        
        actions.Player.Interact.performed += _ => OnInteract?.Invoke();
        actions.Player.Move.performed += ctx => OnMove?.Invoke(ctx.ReadValue<Vector2>());
        actions.Player.Zoom.performed += ctx => OnZoom?.Invoke(ctx.ReadValue<float>());
        actions.Player.SelectCharacter.performed += ctx => OnSelectCharacter?.Invoke((int)ctx.ReadValue<float>());
        actions.Player.Recenter.performed += _ => OnRecenter?.Invoke();
    }

    // private void OnDestroy()
    // {
    //     OnInteract = null;
    //     OnMove = null;
    //     OnZoom = null;
    //     OnSelectCharacter = null; 
    //     OnRecenter = null;
    // }

    private void OnEnable()
    {
        actions.Enable();
    }
    
    private void OnDisable()
    {
        actions.Disable();
    }
}
