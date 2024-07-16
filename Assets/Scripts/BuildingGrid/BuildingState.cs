using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingState : CharacterStates
{
    // Input Fields
    private PlayerInput input;
    private InputAction moveMouseAction, moveCellAction, placeAction, moveOrCancelAction, changeSelectionAction, exitBuildModeAction;
    private Vector2 mousePos;

    // Constructor
    public BuildingState(PlayerController controller) : base(controller) { }

    // Methods
    public override void Enter()
    {
        // Enter build mode
        BuildingGrid.Instance.gameObject.SetActive(true);

        // Initialize inputs
        input = playerController.GetComponent<PlayerInput>();
        moveMouseAction = input.actions.FindAction("MoveMouse");
        moveCellAction = input.actions.FindAction("MoveCell");
        placeAction = input.actions.FindAction("Place");
        moveOrCancelAction = input.actions.FindAction("MoveOrCancel");
        changeSelectionAction = input.actions.FindAction("ChangeSelection");
        exitBuildModeAction = input.actions.FindAction("ExitBuildMode");

        moveMouseAction.Enable();
        moveCellAction.Enable();
        placeAction.Enable();
        moveOrCancelAction.Enable();
        changeSelectionAction.Enable();
        exitBuildModeAction.Enable();

        // Define input behaviors
        moveMouseAction.performed += ctx => BuildingGrid.Instance.MoveToMouseCell();
        moveCellAction.started += ctx =>
        {
            Vector2 dir = ctx.ReadValue<Vector2>();
            BuildingGrid.Instance.MoveCurrentCell((int)dir.x, (int)dir.y);
        };
        placeAction.started += ctx => BuildingGrid.Instance.PlaceBuilding();
        moveOrCancelAction.started += ctx => BuildingGrid.Instance.ToggleMoveBuilding();
        changeSelectionAction.performed += ctx =>
        {
            float axisPos = ctx.ReadValue<float>();
            if (axisPos < 0)
                BuildingGrid.Instance.ChangeBuildSelection(false);
            else if (axisPos > 0)
                BuildingGrid.Instance.ChangeBuildSelection(true);
        };
        exitBuildModeAction.started += ctx => playerController.SwitchStates(new WalkingState(playerController));
    }

    public override void Update()
    {
        // Process inputs
    }

    public override void Exit()
    {
        // Cancel any ongoing build processes
        BuildingGrid.Instance.CancelMoveBuilding();

        // Disable inputs
        moveCellAction.Disable();
        placeAction.Disable();
        moveOrCancelAction.Disable();
        changeSelectionAction.Disable();
        exitBuildModeAction.Disable();

        // Exit build mode
        BuildingGrid.Instance.gameObject.SetActive(false);
    }
}
