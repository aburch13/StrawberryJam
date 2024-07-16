using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControl : MonoBehaviour
{
    // Fields
    public GameObject menu;

    // Methods
    void Awake()
    {
        menu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!menu.activeSelf)
                OpenMenu();
            else
                ExitMenu();
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OpenMenu()
    {
        Time.timeScale = 0;
        menu.SetActive(true);
    }

    public void ExitMenu()
    {
        Time.timeScale = 1f;
        menu.SetActive(false);
    }
}
