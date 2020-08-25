using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class ShipInstance : MonoBehaviour
{
    private PlayerHealthScript trailScript;
    private BoxCollider boxCollider;
    private RealtimeView realtimeView;
    TSyncScript idModel;
    public ScoreManager manager;
    // Start is called before the first frame update
    private bool isInitialized =false;
    public void Initialize(RealtimeView realtimeView)
    {
        this.realtimeView = realtimeView;
        trailScript = GetComponent<PlayerHealthScript>();
        boxCollider = GetComponent<BoxCollider>();

        manager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        idModel = GetComponent<TSyncScript>();

        trailScript.SetHealth(1);

        isInitialized = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isInitialized)
        {
            if (realtimeView.isOwnedLocally)
            {
                boxCollider.enabled = false;
            }
            else
            {
                //reset last hit projectiles ID
                idModel.SetT(-1);

                switch (other.gameObject.layer)
                {
                    //laser
                    case 8:
                        trailScript.SetHealth(trailScript.GetHealth() - 1);
                        //set the owner ID of the projectile, that hit
                        idModel.SetT(other.GetComponent<RealtimeView>().ownerID);
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
}
