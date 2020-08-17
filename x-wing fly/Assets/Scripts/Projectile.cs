using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class Projectile : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<TrailRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        Invoke("Tr", 0.08f);
        Invoke("Col", 0.02f);
        Invoke("Destruct", 1f);
    }

    

    private void Tr()
    {
        GetComponent<TrailRenderer>().enabled = true;
    }
    private void Col()
    {
        GetComponent<BoxCollider>().enabled = true;
    }
    private void Destruct()
    {
        Realtime.Destroy(gameObject);
    }
}
