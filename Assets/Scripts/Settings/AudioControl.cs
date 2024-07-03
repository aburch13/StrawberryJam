using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioControl : MonoBehaviour
{
    // Fields
    public AudioMixer audioMixer;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    // Methods
    void Awake()
    {
        LoadVolumeSettings("masterVolume");
        LoadVolumeSettings("musicVolume");
        LoadVolumeSettings("sfxVolume");
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("masterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetSfxVolume(float volume)
    {
        audioMixer.SetFloat("sfxVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    private void LoadVolumeSettings(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            float volume = PlayerPrefs.GetFloat(key);
            audioMixer.SetFloat(key, Mathf.Log10(volume * 20));
            switch (key)
            {
                case "masterVolume":
                    masterSlider.value = volume;
                    break;
                case "musicVolume":
                    musicSlider.value = volume;
                    break;
                case "sfxVolume":
                    sfxSlider.value = volume;
                    break;
            }
        }
    }
}
