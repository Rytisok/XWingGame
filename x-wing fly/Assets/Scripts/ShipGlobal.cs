using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using UnityEngine.UI;
using TMPro;

public class ShipGlobal : MonoBehaviour
{
  //  public TMP_Text m_Text;
  /*  private void Start()
    {
        if (!transform.root.GetComponent<RealtimeView>().isOwnedLocally)
        {
            m_Text.text = "Player  " + (transform.root.GetComponent<RealtimeView>().ownerID + 1).ToString();
        }
    }

    private void Update()
    {
        m_Text.transform.rotation = Camera.main.transform.rotation;
    }*/
    public void SetState(bool alive)
    {
        GetComponentInChildren<MeshRenderer>().enabled = alive;
    }
}
