using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class Rocket : MonoBehaviour
{
    public GameObject explosion;
    Realtime _realtime;
    Rigidbody rb;
    bool g = false;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _realtime = GameObject.Find("Realtime + VR Player").GetComponent<Realtime>();
        Invoke("Explode", 5);
        Invoke("Trg", 0.2f);
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.forward * 3.7f;
    }

    private void OnTriggerEnter(Collider other)
    {
        Explode();
        g = true;
    }

    void Trg()
    {
        GetComponent<BoxCollider>().enabled = true;
    }

    void Explode()
    {
        if (!g) {
            GameObject expl = Realtime.Instantiate(explosion.name, transform.position, transform.rotation, ownedByClient: true, useInstance: _realtime);
            expl.transform.position = transform.position;
            Realtime.Destroy(gameObject);
        }
    }
}
