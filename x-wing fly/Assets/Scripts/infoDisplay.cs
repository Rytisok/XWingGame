using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Normal.Realtime;

public class infoDisplay : MonoBehaviour
{
    public Text txt;
    public string netInfo;
    // Update is called once per frame
    void Update()
    {
        txt.text = "FPS: " + Mathf.Round((1.0f / Time.deltaTime)).ToString() + "\n" + netInfo;
    }
}
