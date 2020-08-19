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
    void Awake()
    {
        if (spawnedShip == null)
        {
            GameObject temp = GameObject.FindGameObjectWithTag("ShipsController");
            spawnedShip = Instantiate(shipPref, temp.transform.position, Quaternion.Euler(Vector3.zero), temp.transform);
            spawnedShip.name = "Global";
            spawnedShip.GetComponent<ShipInstance>().Initialize(realtimeView);

            temp.GetComponent<ShipsController>().AddGlobalShip(spawnedShip);

        }
    }

    
}
