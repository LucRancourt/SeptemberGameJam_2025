using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;

    public float runSpeed = 40f;

    float horizontalMove = 0f;
    bool jump = false;
    bool dash = false;

    // InputSystem callback
    public void SetMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        horizontalMove = input.x * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
    }

    public void SetJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jump = true;
        }
    }

    public void SetDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            dash = true;
        }
    }

    public void OnFall()
    {
        animator.SetBool("IsJumping", true);
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump, dash);
        jump = false;
        dash = false;
    }
}