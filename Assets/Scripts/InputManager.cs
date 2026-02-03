using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class InputManager : MonoBehaviour
{
    private InputSystem_Actions actions;
    public static event Action OnLmbUp;
    public static event Action OnLmbDown;
    public static event Action OnInteract;
    public static event Action<Vector2> OnMove;
    public static event Action<float> OnZoom;
    public static event Action<int> OnSelectCharacter;
    public static event Action OnRecenter;
    
    private void Awake()
    {
        actions = new InputSystem_Actions();
        actions.Player.Attack.performed += (_) => OnLmbDown?.Invoke();
        actions.Player.Attack.canceled += (_) => OnLmbUp?.Invoke();
        actions.Player.Interact.performed += _ => OnInteract?.Invoke();
        actions.Player.Move.performed += ctx => OnMove?.Invoke(ctx.ReadValue<Vector2>());
        actions.Player.Zoom.performed += ctx => OnZoom?.Invoke(ctx.ReadValue<float>());
        actions.Player.SelectCharacter.performed += ctx => OnSelectCharacter?.Invoke((int)ctx.ReadValue<float>());
        actions.Player.Recenter.performed += _ => OnRecenter?.Invoke();
        
        //OnMove += value => print(value);
        // OnSelectCharacter += value => print(value);
    }

    private void OnDestroy()
    {
        OnLmbDown = null;
        OnLmbUp = null;
        OnInteract = null;
        OnMove = null;
        OnZoom = null;
        OnSelectCharacter = null; 
        OnRecenter = null;
    }

    private void OnEnable()
    {
        actions.Enable();
    }
    
    private void OnDisable()
    {
        actions.Disable();
    }
}
