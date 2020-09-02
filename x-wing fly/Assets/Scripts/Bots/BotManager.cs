using System.Collections;
using System.Collections.Generic;
using Normal.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class BotManager : MonoBehaviour
{
    public GameObject target;

    public Laser laser;

    public float minFollowDistance = 1;
    public float minFireDistance = 7;

    private float currSpeed;
    private float speedBoosted;
    private float speedNormal;

    private float nextTimeReload = 0;
    private int energy;
    private int energyLimit = 20;
    private float timeBetweenEnergyRecovery = 0.15f;

    private const float minDistToRecalculate = 0.5f; 

    public Pursuer pursuer;

    private GameObject lastTarget;
    private Transform lastTargetTrans;

    private bool allowMovement;
    public Text energyTxt;
    void Awake()
    {
        allowMovement = false;
        SetTarget();
        lastTarget = new GameObject("targetPos");
        lastTargetTrans = lastTarget.transform;
        lastTargetTrans.position =  target.transform.position;
        lastTargetTrans.rotation = target.transform.rotation;

    }

   
    void Start()
    {
        LoadSettings();
    }
    void Update()
    {
        if (allowMovement)
        {
            float distanceToTarget = GetDistanceToTarget();

            if (Vector3.Distance(target.transform.position, lastTargetTrans.position) > minDistToRecalculate)
            {

                lastTargetTrans.position = target.transform.position;
                lastTargetTrans.rotation = target.transform.rotation;
                pursuer.RefinePath(lastTargetTrans);
            }

            FireBehaviour(distanceToTarget);

            energyTxt.text = energy.ToString();
        }
    }
    void SetTarget()
    {
        var ship = GameObject.FindGameObjectsWithTag("Ship");
        target = ship[Random.Range(0, ship.Length)];
    }

    void LoadSettings()
    {
        GameLoading loader = GameManager.Instance.GetComponent<GameLoading>();
        RemoteUnityManager unityRemote = GameManager.Instance.GetComponent<RemoteUnityManager>();

        if (!loader.loadingDone)
        {
            unityRemote.onEnergyDataUpdated += UpdateEnergyData;
            unityRemote.onSpeedDataUpdated += UpdateSpeedData;
            loader.onLoadingDone += StartMoveToTarget;
        }
        else
        {
            UpdateEnergyData(unityRemote.energyLimit, unityRemote.timeBetweenEnergyRecovery, true);
            UpdateSpeedData(unityRemote.speedNormal, unityRemote.speedBoosted, true);
            StartMoveToTarget();
        }

    }

    void StartMoveToTarget()
    {

        pursuer.onCondMovement += Movement;
        pursuer.onCondWaitingForAWay += WaitingForAWay;
        pursuer.onCondWaitingForPursuersQueue += WaitingForPursuersQueue;
        pursuer.onCondWaitingForRequest += WaitingForRequest;
        pursuer.onCondWaitingForTheContinuation += WaitingForTheContinuation;
        pursuer.onCondWaitingForWayProcessing += WaitingForWayProcessing;

        lastTargetTrans.position = target.transform.position;
        lastTargetTrans.rotation = target.transform.rotation;
        pursuer.MoveTo(lastTargetTrans);
        allowMovement = true;
    }



    void UpdateEnergyData(int energyLimit, float timeBetweenEnergyRecovery, bool updateFromServer)
    {
        this.energyLimit = energyLimit;
        this.timeBetweenEnergyRecovery = timeBetweenEnergyRecovery;
    }
    void UpdateSpeedData(float speedNormal, float speedBoosted, bool updateFromServer)
    {
        this.speedNormal = speedNormal;
        this.speedBoosted = speedBoosted;
    }

   
    void FollowTarget(float distanceToTarget)
    {
        if (distanceToTarget > minFollowDistance)
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 0.02f);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position), 10 * Time.deltaTime);
    }

    void FireBehaviour(float distanceToTarget)
    {
        if (distanceToTarget < minFireDistance && laser.AllowToFire(energy))
        {
            laser.FireLaser(ref energy);

        }
        else if (energy < energyLimit && Time.time >= nextTimeReload && laser.ShotReloaded() )
        {
            Debug.Log("not fire");

            nextTimeReload = Time.time + timeBetweenEnergyRecovery;
            energy++;
        }
    }

    float GetDistanceToTarget()
    {
        return Vector3.Distance(transform.position, target.transform.position);
    }



   public void WaitingForRequest()
    {
        lastTargetTrans.position = target.transform.position;
        lastTargetTrans.rotation = target.transform.rotation;
        pursuer.MoveTo(lastTargetTrans);
     //   Debug.Log("WaitingForRequest");
    }
   public void WaitingForPursuersQueue()
         {
           //  Debug.Log("WaitingForPursuersQueue");

    }
    public void WaitingForAWay()
        {
         //   Debug.Log("WaitingForAWay");

    }
    public void WaitingForWayProcessing()
         {
           //  Debug.Log("WaitingForWayProcessing");

    }
    public void WaitingForTheContinuation()
         {
             //Debug.Log("WaitingForTheContinuation");

    }

    public void Movement()
         {
             //Debug.Log("Movement");

    }


  /*  private void OnTriggerEnter(Collider other)
    {
        if (GetComponent<RealtimeView>().isOwnedLocally)
        {
            GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            //reset last hit projectiles ID
          //  idModel.SetT(-1);

            switch (other.gameObject.layer)
            {
                //laser
                case 8:
                    Realtime.Destroy(this.gameObject);
                    break;
                //other player
                case 9:
                    Realtime.Destroy(this.gameObject);

                    break;
                //asteroid
                case 11:
                    Realtime.Destroy(this.gameObject);

                    break;
                //orb
                case 14:
               /*     if (trailScript.GetHealth() < trailScript.maxHealth)
                    {
                        trailScript.SetHealth(trailScript.GetHealth() + 1);
                        Realtime.Destroy(other.gameObject);
                    }
                    break;
            }
        }
    }*/
}
