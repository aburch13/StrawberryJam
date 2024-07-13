using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingState : CharacterStates
{
    // Fields
    private PlayerInput input;

    // Constructor
    public BuildingState(PlayerController controller) : base(controller) { }

    // Methods
    public override void Enter()
    {
        // Enter build mode
        BuildingGrid.Instance.gameObject.SetActive(true);

        // Initialize inputs
        input = playerController.gameObject.GetComponent<PlayerInput>();
    }

    public override void Update()
    {
        // Process inputs
    }

    public override void Exit()
    {
        // Cancel any ongoing build processes
        BuildingGrid.Instance.MoveCancel();

        // Exit build mode
        BuildingGrid.Instance.gameObject.SetActive(false);
    }
}
