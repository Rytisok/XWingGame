using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPAudioController : MonoBehaviour
{
    public AudioClip[] clips;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio(int id)
    {
        audioSource.clip = clips[id];
        audioSource.Play();
    }

}
