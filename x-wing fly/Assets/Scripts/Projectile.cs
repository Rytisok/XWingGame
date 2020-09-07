using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class Projectile : MonoBehaviour
{
    public Rigidbody rigidbody;
    public SPShip origin; //sp only
    public void Initialize(float projectileDuration, SPShip origin = null)
    {
        if(origin != null)
        this.origin = origin;
        GetComponent<TrailRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        Invoke("Tr", 0.08f);
        Invoke("Col", 0.02f);
        Invoke("Destruct", projectileDuration);
    }

    public void Fire(Vector3 speed)
    {
        rigidbody.velocity = speed;
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
        if(!GameManager.Instance.offline)
            Realtime.Destroy(gameObject);
        else
            Destroy(gameObject);
        
    }
}
