using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class ShipInstance : MonoBehaviour
{
    public PlayerHealthScript trailScript;
    public BoxCollider boxCollider;
   // public RealtimeView realtimeView;
    public RealtimeTransform realtimeTransform;

    public ScoreManager manager;
    TSyncScript idModel;

    // Start is called before the first frame update
    private bool isInitialized =false;
    public void Initialize(Realtime reference,RealtimeView view, RealtimeTransform trans)
    {
       // realtimeView = view;
       // realtimeTransform = trans;
        //realtimeView._SetRealtime(reference);

    //    GetComponent<RealtimeTransform>().RequestOwnership();
        manager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        idModel = GetComponent<TSyncScript>();

        trailScript.SetHealth(1);

        isInitialized = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isInitialized)
        {
            if (GetComponentInParent<RealtimeView>().isOwnedLocally)
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
