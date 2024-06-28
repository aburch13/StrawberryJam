using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : CharacterStates
{
    CharacterController controller;
    public WalkingState(PlayerController controller) : base(controller)
    {

    }

    public override void Enter()
    {
        controller = playerController.gameObject.GetComponent<CharacterController>();
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
    }
}
