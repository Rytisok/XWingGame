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

    void Awake()
    {
        UnityRemoteManager.Instance.onEnergyDataUpdated += UpdateEnergyData;
        UnityRemoteManager.Instance.onSpeedDataUpdated += UpdateSpeedData;
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

        FollowTarget(distanceToTarget);
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
}
