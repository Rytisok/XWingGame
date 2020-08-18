using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class ShipInstance : MonoBehaviour
{
    public PlayerHealthScript trailScript;
    // Start is called before the first frame update
    void Awake()
    {
        trailScript = GetComponent<PlayerHealthScript>();
        trailScript.SetHealth(1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GetComponentInParent<RealtimeView>().isOwnedLocally)
        {
            GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            switch(other.gameObject.layer)
            {
                //laser
                case 8:
                    trailScript.SetHealth(trailScript.GetHealth() - 1);
                    break;
                //other player
                case 9:
                    trailScript.SetHealth(0);
                    break;
                //asteroid
                case 11:
                    trailScript.SetHealth(0);
                    break;
                //orb
                case 14:
                    if (trailScript.GetHealth() < trailScript.maxHealth)
                    {
                        trailScript.SetHealth(trailScript.GetHealth() + 1);
                        Realtime.Destroy(other.gameObject);
                    }
                    break;
            }
        }
    }
}
