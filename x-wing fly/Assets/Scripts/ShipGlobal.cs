using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class ShipGlobal : MonoBehaviour
{
    public void SetState(bool alive)
    {
        GetComponentInChildren<MeshRenderer>().enabled = alive;
    }
}
