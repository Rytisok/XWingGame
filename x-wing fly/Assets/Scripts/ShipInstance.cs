using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class ShipInstance : MonoBehaviour
{
    private PlayerHealthScript trailScript;
    private BoxCollider boxCollider;
    private RealtimeView realtimeView;
    // Start is called before the first frame update

    public void Initialize(RealtimeView realtimeView)
    {
        this.realtimeView = realtimeView;
        trailScript = GetComponent<PlayerHealthScript>();
        boxCollider = GetComponent<BoxCollider>();

        trailScript.SetHealth(1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (realtimeView.isOwnedLocally)
        {
            boxCollider.enabled = false;
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
