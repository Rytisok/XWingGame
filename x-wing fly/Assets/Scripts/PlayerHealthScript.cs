using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using UnityEngine.UI;
using Unity.RemoteConfig;

public class PlayerHealthScript : RealtimeComponent
{
    private TrailRenderer trail;
    public int maxHealth = 20;
   
    private TrailModel _model;

    private TrailModel model
    {
        set
        {
            if (_model != null)
            {
                _model.healthDidChange -= HealthDidChange;
            }

            _model = value;

            if (_model != null)
            {
                UpdateDisplay();
                _model.healthDidChange += HealthDidChange;
            }
        }
    }
    void Start()
    {
        LoadSettings();
    }

    void LoadSettings()
    {
        GameLoading loader = UnityRemoteManager.Instance.GetComponent<GameLoading>();
        UnityRemoteManager unityRemote = UnityRemoteManager.Instance;

        if (!loader.loadingDone)
            unityRemote.onHealthDataUpdated += UpdateHealth;
        else
        {
            UpdateHealth(unityRemote.maxHealth, true);
        }

    }

    void UpdateHealth(int max, bool isServer)
    {
        maxHealth = max;
    }
    private void HealthDidChange(TrailModel model, int value)
    {
        UpdateDisplay();
    }
    private void UpdateDisplay()
    {
        if(trail !=null)
        trail.time = _model.health * 0.2f;
    }

    public int GetHealth()
    {
        return _model.health;
    }

    public void SetHealth(int h)
    {
        if (_model !=null)
        {
        _model.health = h;

        }
    }
}
