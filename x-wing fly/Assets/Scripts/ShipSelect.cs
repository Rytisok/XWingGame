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
        if (!GameManager.Instance.offline)
            Initialize();
    }

    public void Initialize()
    {
        Debug.Log(loader == null);
        gameObject.SetActive(false);
        loader.onLoadingDone += () =>
        {
            gameObject.SetActive(true);
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!GameManager.Instance.offline)
        {
            if (other.GetComponent<Ship>())
            {
                Ship ship = other.GetComponent<Ship>();
                Select(ship);
            }
        }

    }

    public void Select(Ship ship)
    {
        if (!GameManager.Instance.offline)
        {
            ship.SelectShip(shipNumber);
            ship.enabled = false;
            parent.SetActive(false);
        }
    }
}
