using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager _instance;

    private AudioSource audioSource;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(AudioClip audio)
    {
        _instance.audioSource.clip = audio;
        _instance.audioSource.PlayOneShot(audio);
    }
}