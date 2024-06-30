using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WalkingState : CharacterStates
{
    CharacterController2D controller2D;

    private static float moveSpeed = .004f;
    private InputAction moveAction, interactAction, pauseAction;
    private PlayerInput input;

    private Vector2 moveVector;
    public WalkingState(PlayerController controller) : base(controller)
    {

    }

    public override void Enter()
    {
        controller2D = playerController.gameObject.GetComponent<CharacterController2D>();
        input = playerController.gameObject.GetComponent<PlayerInput>();


        moveAction = input.actions.FindAction("Move");
        interactAction = input.actions.FindAction("Interact");
        pauseAction = input.actions.FindAction("Pause");

        moveAction.Enable();
        interactAction.Enable();
        pauseAction.Enable();

        interactAction.started += ctx => tryInteract();
    }

    public override void Exit()
    {

        moveAction.Disable();
        interactAction.Disable();
        pauseAction.Disable();
    }

    public override void Update()
    {
        ReadInput();
        Move();
    }

    private void ReadInput()
    {
        moveVector = moveAction.ReadValue<Vector2>();
    }

    private void Move()
    {
        Vector3 moveDelta = new Vector3(moveVector.x, moveVector.y);
        moveDelta.Normalize();
        moveDelta *= moveSpeed;
        controller2D.MovePosition(moveDelta);
    }

    private void tryInteract()
    {
        playerController.Interact();
    }
}
