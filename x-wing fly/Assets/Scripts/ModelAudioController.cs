using System.Collections;
using System.Collections.Generic;
using Normal.Realtime;
using UnityEngine;

public class ModelAudioController : RealtimeComponent
{
    public AudioClip[] clips;

    private AudioSource audioSource;
    private ModelAudio _model;
    private ModelAudio model
    {
        set
        {
            if (_model != null)
            {
                // Unregister from events
                _model.clipIDDidChange -= ClipIDDidChange;
            }

            // Store the model
            _model = value;

            if (_model != null)
            {
                // Update the mesh render to match the new model
                PlayAudio(_model.clipID);

                // Register for events so we'll know if the color changes later
                _model.clipIDDidChange += ClipIDDidChange;
            }
        }
    }

    private Transform target;
    private bool initialized;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (initialized)
            transform.position = target.position;
    }

    public void Initialize(Transform target)
    {
        initialized = true;
        this.target = target;

    }

    void ClipIDDidChange(ModelAudio model, int value)
    {
        PlayAudio(model.clipID);
    }

    void PlayAudio(int id)
    {
        if (_model.clipID >= 0)
        {
            audioSource.clip = clips[id];
            audioSource.Play();
        }
    }

    
    public void SetClipID(int id)
    {
        _model.clipID = id;
    }
}
