using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using UnityEngine.UI;
using Unity.RemoteConfig;

public class PlayerHealthScript : RealtimeComponent
{

    private TrailModel _model;
    private TrailRenderer trail;
    public int maxHealth;
    private bool loaded;
    void Awake()
    {
        trail = GetComponent<TrailRenderer>();
    }

    void Start()
    {
        LoadSettings();
    }

    void LoadSettings()
    {
        GameLoading loader = RemoteUnityManager.Instance.GetComponent<GameLoading>();
        RemoteUnityManager unityRemote = RemoteUnityManager.Instance;

        if (!loader.loadingDone)
        {
            unityRemote.onHealthDataUpdated += UpdateHealth;
            loader.onLoadingDone += () => { loaded = true; };
        }

        else
        {
            UpdateHealth(unityRemote.maxHealth, true);
            loaded = true;
        }

    }

    void UpdateHealth(int max, bool isServer)
    {
        maxHealth = max;
    }
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

    private void HealthDidChange(TrailModel model, int value)
    {
        UpdateDisplay();
    }
    private void UpdateDisplay()
    {
        trail.time = _model.health * 0.2f;
    }

    public int GetHealth()
    {
        return _model.health;
    }

    public void SetHealth(int h)
    {
        if(loaded)
        _model.health = h;
    }
}
