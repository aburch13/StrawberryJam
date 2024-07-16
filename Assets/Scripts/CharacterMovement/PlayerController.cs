using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterStates currentState;

    private Interactable nearestInteractable;

    // Start is called before the first frame update
    void Start()
    {
        currentState = new BuildingState(this);
        currentState.Enter();
    }

    // Update is called once per frame
    void Update()
    {
        currentState.Update();
    }

    public void Interact()
    {
        if (nearestInteractable == null)
        {
            CharacterStates s = nearestInteractable.Interact();
            SwitchStates(s);
        }
    }

    public void SwitchStates(CharacterStates s)
    {
        if (s != null)
        {
            currentState.Exit();
            currentState = s;
            s.Enter();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Interactable>() != null)
        {
            nearestInteractable = collision.gameObject.GetComponent<Interactable>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.GetComponent<Interactable>() != null && collision.GetComponent<Interactable>() == nearestInteractable)
        {
            nearestInteractable = null;
        }
    }
}
