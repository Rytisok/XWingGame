﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class OrbManager : MonoBehaviour
{
    public int orbCount;
    public int spawnArea;
    public GameObject orbPrefab;
    public GameObject spOrbPrefab;

    public Realtime _realtime;
    public List<GameObject> orbs = new List<GameObject>();

    float nextCheck = 0;
    float interval = 5;
    bool orbsSpawned = false;
    private bool loaded;
    void Start()
    {

        LoadSettings();
    }

    void LoadSettings()
    {
        GameLoading loader = GameManager.Instance.GetComponent<GameLoading>();

        if (!loader.loadingDone)
        {
            loader.onLoadingDone += () =>
            {
                loaded = true;
            };
        }
        else
        {
           
            loaded = true;
        }

    }


    void Update()
    {
        if (loaded)
        {
            if (!orbsSpawned)
            {
                if (GameManager.Instance.offline)
                {
                    SpawnOrbs();
                }
                else
                {
                    if (_realtime.connected)
                    {
                        SpawnOrbs();
                    }
                }
            }

            if (Time.time > nextCheck)
            {
                for (int i = 0; i < orbCount; i++)
                {
                    if (orbs[i] == null)
                    {
                        orbs.Remove(orbs[i]);
                        Invoke("SpawnOrb", 4);
                        SpawnOrb(i);
                    }
                }

                nextCheck = Time.time + interval;
            }
        }
    }

    void SpawnOrbs()
    {
        for (int i = 0; i < orbCount; i++)
        {
            SpawnOrb(i);
        }

        orbsSpawned = true;

    }

    void SpawnOrb(int n)
    {
        Vector3 pos = (Random.insideUnitSphere * spawnArea) + transform.position;
        GameObject orb;
        if (!GameManager.Instance.offline)
        {
             orb = Realtime.Instantiate(orbPrefab.name, pos, transform.rotation, ownedByClient: false,
                useInstance: _realtime);
        }
        else
        {
            orb = Instantiate(spOrbPrefab, pos, transform.rotation,transform);
        }
        if (orb != null)
        {
            orb.transform.position = pos;
            orbs.Insert(n, orb);
        }
        else
        {
            Debug.LogWarning("No orb created!");
        }
    }
}
