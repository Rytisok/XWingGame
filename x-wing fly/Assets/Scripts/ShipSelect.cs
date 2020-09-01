using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class ShipSelect : MonoBehaviour
{
    public GameLoading loader;
    public Realtime _realtime;
    public GameObject parent;
    public int shipNumber;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        loader.onLoadingDone += () =>
        {
            gameObject.SetActive(true);
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Ship>())
        {
            other.GetComponent<Ship>().SelectShip(shipNumber);
            other.enabled = false;
            parent.SetActive(false);
        }
    }
}
