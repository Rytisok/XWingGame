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
    public Realtime _realtime;

    void Awake()
    {
        if (spawnedShip == null)
        {
            GameObject temp = GameObject.FindGameObjectWithTag("ShipsController");
            spawnedShip = Realtime.Instantiate(shipPref.name, temp.transform.position, Quaternion.Euler(Vector3.zero),true,false,true,_realtime);
            spawnedShip.name = "Global";
            spawnedShip.transform.parent = temp.transform;
            spawnedShip.GetComponent<ShipInstance>().Initialize(realtimeView);

            temp.GetComponent<ShipsController>().AddGlobalShip(spawnedShip);

        }
    }

    
}
