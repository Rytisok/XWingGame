using System;
using System.Collections;
using System.Collections.Generic;
using Unity.RemoteConfig;
using UnityEngine;

public class RemoteUnityManager : MonoBehaviour
{
  public Action<float, float, float> onLaserDataUpdated;
  public Action<float, float> onSpeedDataUpdated;
  public Action<int, float,float,float> onEnergyDataUpdated;
  public Action<int> onHealthDataUpdated;
  public Action<int> onBotDataUpdated;

    public float projectileSpeed;
    public float minIntervalBetweenShots;
    public float projectileDuration;
    public float speedBoosted;
    public float speedNormal;
    public int maxEnergy;
    public int maxHealth;
    public float energyRecoveryInterval;
    public int botAccuracy;
    public float boostCost;
    public float boostUsageInterval;

    private Action<PartsToLoad> succCallback;

    public struct userAttributes
    {
    }

    public struct appAttributes
    {
    }

  

    public void StartLoading(Action<PartsToLoad> callback)
    {
        succCallback = callback;

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
                break;
        }
        speedNormal = ConfigManager.appConfig.GetFloat("speedNormal");
        speedBoosted = ConfigManager.appConfig.GetFloat("speedBoosted");
        projectileSpeed = ConfigManager.appConfig.GetFloat("projectileSpeed");
        minIntervalBetweenShots = ConfigManager.appConfig.GetFloat("minIntervalBetweenShots");
        projectileDuration = ConfigManager.appConfig.GetFloat("projectileDuration");
        maxEnergy = ConfigManager.appConfig.GetInt("maxEnergy");
        energyRecoveryInterval = ConfigManager.appConfig.GetFloat("energyRecoveryInterval");
        maxHealth = ConfigManager.appConfig.GetInt("maxHealth");
        botAccuracy = ConfigManager.appConfig.GetInt("botAccuracy");
        boostCost = ConfigManager.appConfig.GetInt("boostCost");
        boostUsageInterval = ConfigManager.appConfig.GetInt("boostUsageInterval");

        onLaserDataUpdated?.Invoke(projectileSpeed, projectileDuration, minIntervalBetweenShots);
        onSpeedDataUpdated?.Invoke(speedNormal, speedBoosted);
        onEnergyDataUpdated?.Invoke(maxEnergy, energyRecoveryInterval, boostCost, boostUsageInterval);
        onHealthDataUpdated?.Invoke(maxHealth);
        onBotDataUpdated?.Invoke(botAccuracy);

        succCallback?.Invoke(PartsToLoad.UnityRemote);
    }
}
