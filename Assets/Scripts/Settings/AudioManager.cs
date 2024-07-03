using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Fields
    [Header("--- Sources ---")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("--- BGM Clips ---")]
    public AudioClip bgm;

    [Header("--- SFX Clips ---")]
    public AudioClip sfx1;
    public AudioClip sfx2;

    // Properties
    public static AudioManager Instance
    {
        get;
        private set;
    }

    // Methods
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Start BGM loop if present
        if (bgm != null)
        {
            musicSource.clip = bgm;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlaySFX(AudioClip sfx)
    {
        if (sfx != null)
            sfxSource.PlayOneShot(sfx);
    }
}
