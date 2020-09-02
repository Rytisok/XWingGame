using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class SPPlayerHealth : MonoBehaviour
{
    private TrailRenderer trail;
    public int maxHealth;
    public int currentHealth { get; private set; }
    private bool loaded;
    void Awake()
    {
        trail = GetComponent<TrailRenderer>();
    }

    void Start()
    {
        ResetValues();
        LoadSettings();
    }

    public void ResetValues()
    {
        currentHealth = 1;
    }

    public bool IsAtMaxHP()
    {
        return currentHealth >= maxHealth;
    }

    void LoadSettings()
    {
        GameLoading loader = GameManager.Instance.GetComponent<GameLoading>();
        RemoteUnityManager unityRemote = GameManager.Instance.GetComponent<RemoteUnityManager>();

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
 
    private void UpdateDisplay()
    {
        trail.time = currentHealth * 0.2f;
    }

    public void ChangeHealth(int change)
    {
        if (loaded)
        {
            currentHealth += change;
            UpdateDisplay();
        }
    }
}