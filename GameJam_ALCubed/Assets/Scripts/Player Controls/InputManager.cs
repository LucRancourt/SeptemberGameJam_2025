using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    private InputSystem_Actions _actions;
    private Camera _mainCamera;

    //Mouse-Click Actions
    public InputAction ClickAction { get; private set; }

    public event Action OnMouseRelease;

    void OnEnable()
    {
        _mainCamera = Camera.main;
        _actions = new InputSystem_Actions();
        _actions.DragAndDrop.Enable();

        ClickAction = _actions.DragAndDrop.Click;
        ClickAction.started += OnClickPerformed;
        ///ClickAction.performed += OnClickPerformed;
        ClickAction.canceled += OnClickReleased;
    }

    void OnDisable()
    {
        //_actions.DragAndDrop.Disable();
    }

    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        RaycastHit2D hit = Physics2D.Raycast(_mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero);

        if (hit)
        {
            hit.collider.gameObject.GetComponent<IDraggable>()?.OnClick();
        }
    }

    private void OnClickReleased(InputAction.CallbackContext context)
    {
        OnMouseRelease?.Invoke();
    }
}