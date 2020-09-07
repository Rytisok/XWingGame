using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupShipForNonVR : MonoBehaviour
{
    public Transform shipModel;
    void Start()
    {
        if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer)
            SetupForNonVR();
    }

    void SetupForNonVR()
    {
        shipModel.localPosition = new Vector3(0, -0.207f, 0.389f);
        shipModel.localRotation = Quaternion.Euler(new Vector3(21.6f, 0, 0));
    }
}
