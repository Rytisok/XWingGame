using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Normal.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BotManager : MonoBehaviour
{

    public Laser laser;
    public Pursuer pursuer;
    public FiringPrediction firingPrediction;

    public float minFollowDistance = 1;
    public float minFireDistance = 7;

    private float currSpeed;
    private const float speedMin = 0.01f;
    private const float speedMax = 0.03f;


    private float nextTimeReload = 0;
    private float energy;
    private int energyLimit = 20;
    private float timeBetweenEnergyRecovery = 0.15f;

    private float distToRecalculate = 0.5f;
    private const float mindistToRecalculate = 0.2f;
    private const float maxdistToRecalculate = 0.5f;

    public GameObject target;
    private Vector3 lastTargetPos;

    private bool allowShooting;
    private bool allowMovement;
    public Text energyTxt;

    private SPShip spShip;
    private SPShip targetShip;

    const string playerTag = "Ship";
    const string enemyBot = "BotEnemy";

    private int botAccuracy;
    void Awake()
    {
        spShip = GetComponent<SPShip>();

        allowMovement = false;
        allowShooting = false;
        SetARandomTarget();

        spShip.onDeath += OnDeath;
        spShip.onRespawn += OnRespawn;


        ResetTargetParameters();

    }

    private void OnDeath()
    {
        allowMovement = false;
        allowShooting = false;

        pursuer.ResetCondition();
    }

    private void OnRespawn()
    {
        allowMovement = true;
        allowShooting = true;

        SetARandomTarget();
        ResetTargetParameters();
        
        pursuer.MoveTo(target.transform);
    }


    void ResetTargetParameters()
    {
        lastTargetPos = target.transform.position;
        allowShooting = true;
    }

    void Reset()
    {
        SetRandomSpeed();
        SetRecalculationDistance();
    }

   
    void Start()
    {
        LoadSettings();
    }
    void Update()
    {
        if (allowMovement)
        {
            if (Vector3.Distance(target.transform.position, lastTargetPos) > distToRecalculate)
            {
                ResetTargetParameters();
                pursuer.RefinePath(target.transform);
            }
            if(allowShooting)
                FireBehaviour(Vector3.Distance(transform.position, target.transform.position));

            energyTxt.text = energy.ToString();
        }
    }

    private void TargetDead()
    {
        targetShip.onDeath -= TargetDead;
        SetARandomTarget();
        ResetTargetParameters();
        pursuer.MoveTo(target.transform);
    }

    void SetRandomSpeed()
    {
        currSpeed = Random.Range(speedMin, speedMax);
        pursuer.speed = currSpeed;
    }
    void SetRecalculationDistance()
    {
        distToRecalculate = Random.Range(mindistToRecalculate, maxdistToRecalculate);
    }
    void SetARandomTarget()
    {
     
        var players = GameObject.FindGameObjectsWithTag(playerTag); // get players
        var bots = GameObject.FindGameObjectsWithTag(enemyBot).Where(x => x != gameObject).ToArray();
        GameObject[] allViableTargets = new GameObject[players.Length + bots.Length];
        Array.Copy(players, allViableTargets, players.Length);
        Array.Copy(bots, 0, allViableTargets, players.Length, bots.Length);

        target = allViableTargets[Random.Range(0, allViableTargets.Length)];

        if (target.tag == playerTag)
            targetShip = GetComponentInChildren<SPShip>();
        else
            targetShip = GetComponent<SPShip>();

        Debug.Log(targetShip.tag);

        targetShip.onDeath += TargetDead;

    }

    void LoadSettings()
    {
        GameLoading loader = GameManager.Instance.GetComponent<GameLoading>();
        RemoteUnityManager unityRemote = GameManager.Instance.GetComponent<RemoteUnityManager>();

        if (!loader.loadingDone)
        {
            unityRemote.onEnergyDataUpdated += UpdateEnergyData;
            unityRemote.onBotDataUpdated += UpdateBotData;

            loader.onLoadingDone += () =>
            {
                InitializeFiringPrediction(unityRemote.projectileSpeed, unityRemote.botAccuracy);
                StartMoveToTarget();
            };
        }
        else
        {
            UpdateEnergyData(unityRemote.energyLimit, unityRemote.timeBetweenEnergyRecovery, unityRemote.boostCost, true);
            InitializeFiringPrediction(unityRemote.projectileSpeed, unityRemote.botAccuracy);
            StartMoveToTarget();
        }

    }

    void InitializeFiringPrediction(float projectileSpeed,int accuracy)
    {
        firingPrediction.Initialize(laser.laserOrigin.gameObject,target.gameObject,projectileSpeed, minFireDistance, accuracy);

    }

    void StartMoveToTarget()
    {

        pursuer.onCondMovement += Movement;
        pursuer.onCondWaitingForAWay += WaitingForAWay;
        pursuer.onCondWaitingForPursuersQueue += WaitingForPursuersQueue;
        pursuer.onCondWaitingForRequest += WaitingForRequest;
        pursuer.onCondWaitingForTheContinuation += WaitingForTheContinuation;
        pursuer.onCondWaitingForWayProcessing += WaitingForWayProcessing;

        ResetTargetParameters();
        pursuer.MoveTo(target.transform);
        allowMovement = true;
        allowShooting = true;
    }



    void UpdateEnergyData(int energyLimit, float timeBetweenEnergyRecovery, float boostCost, bool updateFromServer)
    {
        this.energyLimit = energyLimit;
        this.timeBetweenEnergyRecovery = timeBetweenEnergyRecovery;
    }

    void UpdateBotData(int botAccuracy, bool updateFromServer)
    {
        this.botAccuracy = botAccuracy;
    }


    /* void FollowTarget(float distanceToTarget)
     {
         if (distanceToTarget > minFollowDistance)
             transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 0.02f);

         transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position), 10 * Time.deltaTime);
     }*/

    void FireBehaviour(float distanceToTarget)
    {
        if (distanceToTarget < minFireDistance && laser.AllowToFire(energy))
        {
            laser.FireLaser(ref energy);

        }
        else if (energy < energyLimit && Time.time >= nextTimeReload && laser.ShotReloaded() )
        {

            nextTimeReload = Time.time + timeBetweenEnergyRecovery;
            energy++;
        }
    }


   public void WaitingForRequest()
   {
       ResetTargetParameters();
       if(allowMovement)
        pursuer.MoveTo(target.transform);
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
}
