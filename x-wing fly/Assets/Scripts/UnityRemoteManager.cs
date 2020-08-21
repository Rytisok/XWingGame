using System;
using System.Collections;
using System.Collections.Generic;
using Unity.RemoteConfig;
using UnityEngine;

public class UnityRemoteManager : MonoBehaviour
{
    public Action<float, float, float, bool> onLaserDataUpdated;
    public Action<float, float, bool> onSpeedDataUpdated;
    public Action<int, float, bool> onEnergyDataUpdated;

    private float projectileSpeed;
    private float timeBetweenShots;
    private float projectileDuration;
    private float speedBoosted;
    private float speedNormal;
    private int energyLimit;
    private float timeBetweenEnergyRecovery;

    public struct userAttributes
    {
    }

    public struct appAttributes
    {
    }

    #region UnityRemoteManager Instance

    private static UnityRemoteManager instance = null;

    public static UnityRemoteManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        Application.targetFrameRate = 60;

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;//Avoid doing anything else
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    void Start()
    {
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

                speedNormal = ConfigManager.appConfig.GetFloat("speedNormal");
                speedBoosted = ConfigManager.appConfig.GetFloat("speedBoosted");
                projectileSpeed = ConfigManager.appConfig.GetFloat("projectileSpeed");
                timeBetweenShots = ConfigManager.appConfig.GetFloat("timeBetweenShots");
                projectileDuration = ConfigManager.appConfig.GetFloat("projectileDuration");
                energyLimit = ConfigManager.appConfig.GetInt("energyLimit");
                timeBetweenEnergyRecovery = ConfigManager.appConfig.GetFloat("timeBetweenEnergyRecovery");

                onLaserDataUpdated?.Invoke(projectileSpeed,projectileDuration,timeBetweenShots,true);
                onSpeedDataUpdated?.Invoke(speedNormal,speedBoosted,true);
                onEnergyDataUpdated?.Invoke(energyLimit,timeBetweenEnergyRecovery,true);

                break;
        }
    }
}
