using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using UnityEngine.UI;
using Unity.RemoteConfig;

public class PlayerHealthScript : RealtimeComponent
{
    public struct userAttributes
    {
    }

    public struct appAttributes
    {
    }
    private TrailModel _model;
    private TrailRenderer trail;
    public int maxHealth;
    void Awake()
    {
        trail = GetComponent<TrailRenderer>();
        // Add a listener to apply settings when successfully retrieved: 
        ConfigManager.FetchCompleted += ApplyRemoteSettings;

        // Set the user’s unique ID:
        ConfigManager.SetCustomUserID("some-user-id");

        // Fetch configuration setting from the remote service: 
        ConfigManager.FetchConfigs<userAttributes, appAttributes>(new userAttributes(), new appAttributes());
    }

    void ApplyRemoteSettings(ConfigResponse configResponse)
    {
        // Conditionally update settings, depending on the response's origin:
        switch (configResponse.requestOrigin)
        {
            case ConfigOrigin.Default:
                Debug.Log("No settings loaded this session; using default values.");
                break;
            case ConfigOrigin.Cached:
                Debug.Log("No settings loaded this session; using cached values from a previous session.");

                break;
            case ConfigOrigin.Remote:
                Debug.Log("New settings loaded this session; update values accordingly.");
                maxHealth = ConfigManager.appConfig.GetInt("maxHealth");
                break;
        }
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
        _model.health = h;
    }
}
