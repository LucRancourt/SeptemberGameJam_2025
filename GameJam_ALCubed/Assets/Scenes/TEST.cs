
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TEST : MonoBehaviour
{
    // Variables
    #region Actions
    public event Action<Vector2> MoveEvent;

    // Camera Movement Events
    public event Action<Vector2> LookEvent;
    public event Action<bool> RotateCamEvent;
    public event Action<Vector2> ZoomEvent;

    public event Action PauseGameEvent;
    #endregion


    // Functions
    #region Handlers
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(Vector2.zero);
    }

    private void OnLookPerformed(InputAction.CallbackContext context)
    {
        LookEvent?.Invoke(context.ReadValue<Vector2>());
    }

    private void OnRotateCamPerformed(InputAction.CallbackContext context)
    {
        RotateCamEvent?.Invoke(context.ReadValueAsButton());
    }

    private void OnZoomPerformed(InputAction.CallbackContext context)
    {
        ZoomEvent?.Invoke(context.ReadValue<Vector2>());
    }

    private void OnPauseGamePerformed(InputAction.CallbackContext context)
    {
        PauseGameEvent?.Invoke();
    }
    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //YEEEEEHAW
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
