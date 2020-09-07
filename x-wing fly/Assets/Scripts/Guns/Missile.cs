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

    public void LaunchMissile()
    {
        if (missileInControl == null)
        {
            GameObject missile;
            if (!GameManager.Instance.offline)
            {
                 missile = Realtime.Instantiate(missilePref.name, missileOrigin.position,
                    missileOrigin.rotation,
                    ownedByClient: true, useInstance: _realtime);
            }
            else
            {

                missile = Realtime.Instantiate(missilePref.name, missileOrigin.position, missileOrigin.rotation);
            }

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
