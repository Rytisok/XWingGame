using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class SelectionOrb : MonoBehaviour
{
    public GameLoading gameLoading;
    public Realtime _realtime;
    public string connectToName;
    public GameObject otherOrb;

    private void OnTriggerEnter(Collider other)
    {
        _realtime.SetRoomName(connectToName);
        gameObject.SetActive(false);
        otherOrb.SetActive(false);
        other.enabled = false;
        gameLoading.StartLoading();
    }
}
