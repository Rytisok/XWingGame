using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class SelectionOrb : MonoBehaviour
{
    public string connectToName;
    public GameObject otherOrb;
    public bool connectionOrb;
    public GameObject nextSelection;

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
        otherOrb.SetActive(false);
        if (connectionOrb)
        {
            GameManager.Instance.GetComponent<GameLoading>().LoadWithRoom(connectToName);
        }
        else
        {
            nextSelection.SetActive(true);
        }  
    }
}
