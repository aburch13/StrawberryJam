using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterStates 
{
    protected PlayerController playerController;

    public CharacterStates(PlayerController controller)
    {
        this.playerController = controller;
    }

    public abstract void Enter();
    public abstract void Exit();
    public abstract void Update();
}
