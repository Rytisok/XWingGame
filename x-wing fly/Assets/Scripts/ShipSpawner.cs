using System.Collections;
using System.Collections.Generic;
using Normal.Realtime;
using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
    public GameObject shipPref;
    [HideInInspector]
    public GameObject spawnedShip;

    public RealtimeView realtimeView;

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
        if (spawnedShip == null)
        {
            GameObject temp = GameObject.FindGameObjectWithTag("ShipsController");
            ShipsController shipsController = temp.GetComponent<ShipsController>();

            spawnedShip = Realtime.Instantiate(shipPref.name, temp.transform.position, Quaternion.Euler(Vector3.zero), true, false, true, shipsController._realtime);
            spawnedShip.name = "Global";
            spawnedShip.transform.parent = temp.transform;
            spawnedShip.GetComponent<ShipInstance>().Initialize(realtimeView);

            shipsController.AddGlobalShip(spawnedShip);

        }
    }
}
