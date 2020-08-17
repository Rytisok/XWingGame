using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class PlayerAudioScript : RealtimeComponent
{
    
    private AudioModel _model;
    public AudioSource source;
    RealtimeView view;
    public AudioClip[] audioClips;
    public int curVal;

    private void Awake()
    {
        view = GetComponent<RealtimeView>();
    }

    private AudioModel model
    {
        set
        {
            if(_model != null)
            {
                _model.audClpDidChange -= AudClpDidChange;
            }
            _model = value;

            

            if (_model != null)
            {
                PlayAudio();
                _model.audClpDidChange += AudClpDidChange;
            }
        }
        
    }

    private void AudClpDidChange(AudioModel model, int value)
    {
        PlayAudio();
    }

    private void PlayAudio()
    {
        if (_model.audClp != -1)
        {
            source.Stop();
            source.clip = audioClips[_model.audClp];
            source.Play();
        }

    }
    public void PlaySound(int pl)
    {
        _model.audClp = pl;
    }
}
