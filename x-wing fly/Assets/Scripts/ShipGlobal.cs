using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using UnityEngine.UI;
using TMPro;

public class ShipGlobal : MonoBehaviour
{
    public TMP_Text m_Text;
    public GameObject[] shipModels;
    private void Start()
    {
        RealtimeView view = transform.root.GetComponent<RealtimeView>();
        if (!view.isOwnedLocally)
        {
            m_Text.text = "Player  " + (view.ownerID + 1).ToString();
        }
        if (view.realtime.GetCurrentRoomName() == "t")
        {
            if (transform.root.GetComponent<RealtimeView>().ownerID % 2 != 0)
            {
                shipModels[0].SetActive(false);
                shipModels[1].SetActive(true);
            }
        }
    }

    private void Update()
    {
        m_Text.transform.rotation = Camera.main.transform.rotation;
    }
    public void SetState(bool alive)
    {
        GetComponentInChildren<MeshRenderer>().enabled = alive;
    }
}
