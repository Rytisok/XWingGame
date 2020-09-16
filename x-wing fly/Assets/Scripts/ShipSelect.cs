using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using TMPro;

public class ShipSelect : MonoBehaviour
{
    public GameLoading loader;
    public Realtime _realtime;
    public GameObject parent;
    public Ship ship;
    public Collider shipCollider;
    public int shipNumber;
    public TMP_Text chooseText;
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
            if (_realtime.GetCurrentRoomName() == "d")
            {
                chooseText.text = "Select skin";
            }
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!GameManager.Instance.offline)
        {
            Select();
        }

    }

    public void Select()
    {
        if (!GameManager.Instance.offline)
        {
            ship.SelectShip(shipNumber);
            shipCollider.enabled = false;
            parent.SetActive(false);
        }
    }
}
