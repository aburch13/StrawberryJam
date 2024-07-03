using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ResolutionControl : MonoBehaviour
{
    // Fields
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown windowDropdown;
    private List<Resolution> resolutions;

    private double refreshRate;
    private int selectionIndex;

    // Methods
    void Awake()
    {
        // Generate list of resolution options based on user's monitor refresh rate
        resolutions = new List<Resolution>();
        refreshRate = Screen.currentResolution.refreshRateRatio.value;
        resolutionDropdown.ClearOptions();

        // Filter for resolutions matching current refresh rate
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].refreshRateRatio.value == refreshRate)
                resolutions.Add(Screen.resolutions[i]);
        }

        // Create labels for available options
        // Set selection index to current resolution
        List<string> optionLabels = new List<string>();
        for (int i = 0; i < resolutions.Count; i++)
        {
            optionLabels.Add(resolutions[i].width + "x" + resolutions[i].height);
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
                selectionIndex = i;
        }

        // Update dropdown with labels and current selection
        resolutionDropdown.AddOptions(optionLabels);
        resolutionDropdown.value = selectionIndex;
        resolutionDropdown.RefreshShownValue();

        // Load any existing resolution settings
        if (PlayerPrefs.HasKey("resolution"))
        {
            int index = PlayerPrefs.GetInt("resolution");
            SetResolution(index);
            resolutionDropdown.value = index;
            resolutionDropdown.RefreshShownValue();
        }
        if (PlayerPrefs.HasKey("windowMode"))
        {
            int index = PlayerPrefs.GetInt("windowMode");
            SetWindowSettings(index);
            windowDropdown.value = index;
            windowDropdown.RefreshShownValue();
        }
    }

    public void SetResolution(int index)
    {
        Screen.SetResolution(resolutions[index].width, resolutions[index].height, Screen.fullScreen);
        PlayerPrefs.SetInt("resolution", index);
    }

    public void SetWindowSettings(int index)
    {
        switch (index)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;

            case 1:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }

        PlayerPrefs.SetInt("windowMode", index);
    }
}
