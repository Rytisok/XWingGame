using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class TimedDestroy : MonoBehaviour
{
    public float t;
    private void Awake()
    {
        Realtime.Destroy(gameObject, t);
    }
}
