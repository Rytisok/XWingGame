using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class SelectionOrb : MonoBehaviour
{
    public GameLoading gameLoading;
    public string connectToName;
    public GameObject otherOrb;

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
        otherOrb.SetActive(false);
        gameLoading.LoadWithRoom(connectToName);
    }
}
