using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterStates currentState;

    [SerializeField]
    private InputAction moveAction, interactAction, pauseAction;
    [SerializeField]
    private PlayerInput input;

    // Start is called before the first frame update
    void Start()
    {
        moveAction = input.actions.FindAction("Move");
        moveAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        currentState.Update();
    }
}
