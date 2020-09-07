using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Debug = UnityEngine.Debug;

public class ShipInstance : MonoBehaviour
{
    public PlayerHealthScript trailScript;
    public ScoreManager manager;
    TSyncScript idModel;

    public Action onOrbPickup;

    // Start is called before the first frame update
    void Awake()
    {
        trailScript = GetComponent<PlayerHealthScript>();
        manager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        idModel = GetComponent<TSyncScript>();

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
            Debug.Log("SHIP HIT "+ other.gameObject.layer);

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

                //rocket explosion
                case 12:
                    trailScript.SetHealth(trailScript.GetHealth() - 1);
                    //set the owner ID of the projectile, that hit
                    idModel.SetT(other.GetComponent<RealtimeView>().ownerID);
                    break;

                //rocket direct hit
                case 13:
                    trailScript.SetHealth(0);
                    //set the owner ID of the projectile, that hit
                    idModel.SetT(other.GetComponent<RealtimeView>().ownerID);
                    break;

                //orb
                case 14:
                    if (trailScript.GetHealth() < trailScript.maxHealth)
                    {
                        trailScript.SetHealth(trailScript.GetHealth() + 1);
                    }
                    onOrbPickup?.Invoke();

                    Realtime.Destroy(other.gameObject);
                    break;
            }
        }
    }

}
