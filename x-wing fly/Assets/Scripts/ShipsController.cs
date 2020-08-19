using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipsController : MonoBehaviour
{

    public GameObject localShip;
    public GameObject globalShip;

    public Transform rightHandAnchor;

    public Fly fly;
    void Awake()
    {
        localShip.GetComponent<Ship>().Initialize(fly);
        Move(fly.transform.position);
        fly.onChangePos += Move;
    }

    public void AddGlobalShip(GameObject ship)
    {
        globalShip = ship;
        ResetRotations();
    }

    void Move(Vector3 targetPos)
    {

        transform.position = rightHandAnchor.position;
        transform.rotation = rightHandAnchor.rotation;
    }


    void FixedUpdate()
    {
        ResetRotations();
    }
    void ResetRotations()
    {
        localShip.transform.localRotation = Quaternion.Euler(Vector3.zero);
        if (globalShip != null)
            globalShip.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }


}
