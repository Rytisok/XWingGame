using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    void Awake()
    {
        lastTarget = new GameObject("targetPos");
        lastTargetTrans = lastTarget.transform;
        lastTargetTrans.position =  target.transform.position;
        lastTargetTrans.rotation = target.transform.rotation;

        StartCoroutine(WaitUntilUnityRemoteOnline());
        StartCoroutine(WaitUntilGraphProcessingDone());

        pursuer.onCondMovement += Movement;
        pursuer.onCondWaitingForAWay += WaitingForAWay;
        pursuer.onCondWaitingForPursuersQueue += WaitingForPursuersQueue;
        pursuer.onCondWaitingForRequest += WaitingForRequest;
        pursuer.onCondWaitingForTheContinuation += WaitingForTheContinuation;
        pursuer.onCondWaitingForWayProcessing += WaitingForWayProcessing;
    }

    IEnumerator WaitUntilUnityRemoteOnline()
    {
        while (UnityRemoteManager.Instance == null)
            yield return null;

        UnityRemoteManager.Instance.onEnergyDataUpdated += UpdateEnergyData;
        UnityRemoteManager.Instance.onSpeedDataUpdated += UpdateSpeedData;
    }

    IEnumerator WaitUntilGraphProcessingDone()
    {
        SpaceManager spaceManagerInstance = Component.FindObjectOfType<SpaceManager>();
        while (!spaceManagerInstance.isPrimaryProcessingCompleted)
            yield return null;

        lastTargetTrans.position = target.transform.position;
        lastTargetTrans.rotation = target.transform.rotation;
        pursuer.MoveTo(lastTargetTrans);
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

    void Update()
    {
        float distanceToTarget = GetDistanceToTarget();

        if (Vector3.Distance(target.transform.position, lastTargetTrans.position) > minDistToRecalculate)
        {

            lastTargetTrans.position = target.transform.position;
            lastTargetTrans.rotation = target.transform.rotation;
            pursuer.RefinePath(lastTargetTrans);
        }
        FireBehaviour(distanceToTarget);
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
            laser.FireLaser(ref energy);
        else if (energy < energyLimit && Time.time >= nextTimeReload && laser.ShotReloaded() )
        {
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
        Debug.Log("WaitingForRequest");
    }
   public void WaitingForPursuersQueue()
         {
             Debug.Log("WaitingForPursuersQueue");

    }
    public void WaitingForAWay()
        {
            Debug.Log("WaitingForAWay");

    }
    public void WaitingForWayProcessing()
         {
             Debug.Log("WaitingForWayProcessing");

    }
    public void WaitingForTheContinuation()
         {
             Debug.Log("WaitingForTheContinuation");

    }

    public void Movement()
         {
             Debug.Log("Movement");

    }
}
