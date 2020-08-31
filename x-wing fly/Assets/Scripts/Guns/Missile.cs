using System.Collections;
using System.Collections.Generic;
using Normal.Realtime;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public Realtime _realtime;

    public GameObject missilePref;
    public Transform missileOrigin;
    public Transform controller;

    private GameObject missileInControl;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void LaunchMissile()
    {
        if (missileInControl == null)
        {
            GameObject missile = Realtime.Instantiate(missilePref.name, missileOrigin.position, missileOrigin.rotation,
                ownedByClient: true, useInstance: _realtime);
            missileInControl = missile;
        }
    }

    void MissileControler()
    {
        missileInControl.transform.rotation = controller.rotation;
    }

    void Update()
    {
        if (missileInControl != null)
        {
            MissileControler();
        }
    }
}
