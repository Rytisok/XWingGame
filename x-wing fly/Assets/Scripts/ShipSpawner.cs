﻿using System.Collections;
using System.Collections.Generic;
using Normal.Realtime;
using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
    public GameObject shipPref;
    [HideInInspector]
    public GameObject globalShip;

    void Start()
    {
        LoadSettings();
    }

    void LoadSettings()
    {
        GameLoading loader = UnityRemoteManager.Instance.GetComponent<GameLoading>();

        if (!loader.loadingDone)
            loader.onLoadingDone += NewGlobalShip;
        else
        {
            NewGlobalShip();

        }

    }

    void NewGlobalShip()
    {

        //     GameObject temp = GameObject.FindGameObjectWithTag("ShipsController");
        //  ShipsController shipsController = temp.GetComponent<ShipsController>();

        //    spawnedShip = Realtime.Instantiate(shipPref.name, temp.transform.position, Quaternion.Euler(Vector3.zero), true, false, true, shipsController._realtime);
        //     spawnedShip.name = "Global";
        //    spawnedShip.transform.parent = temp.transform;
      //  globalShip.GetComponent<ShipInstance>().Initialize(shipsController._realtime, transform.root.GetComponent<RealtimeView>(), transform.root.GetComponent<RealtimeTransform>());

           // shipsController.AddGlobalShip(spawnedShip);

        
    }
}
