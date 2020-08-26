using System.Collections;
using System.Collections.Generic;
using Normal.Realtime;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public Realtime _realtime;

    public GameObject missilePref;
    public Transform missileOrigin;

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
        float x = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x * 1.65f;
        float y = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y * 1.65f;

        missileInControl.transform.Rotate(-y, x, 0);
    }

    void Update()
    {
        if (missileInControl != null)
        {
            MissileControler();
        }
    }
}
