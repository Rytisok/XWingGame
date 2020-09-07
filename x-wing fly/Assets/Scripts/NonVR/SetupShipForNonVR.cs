using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupShipForNonVR : MonoBehaviour
{
    public Camera NonVRCamera;
    public Transform shipModel;

    public bool setupCamera;
    void Start()
    {
        if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            SetupForNonVR();
            if(setupCamera)
            SetupCamera();
        }
    }

    void SetupCamera()
    {
        NonVRCamera.fieldOfView = 60;
    }
    void SetupForNonVR()
    {
        shipModel.localPosition = new Vector3(0, -0.166f, 0.548f);
        shipModel.localRotation = Quaternion.Euler(new Vector3(-1.75f, 0, 0));
    }
}
