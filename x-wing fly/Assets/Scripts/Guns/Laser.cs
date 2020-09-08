using System;
using System.Collections;
using System.Collections.Generic;
using Normal.Realtime;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Realtime _realtime;
    public GameObject[] laserVariants;
    public GameObject laserPref;
    public Transform laserOrigin;

    public Action<int> onPlaySound;

    private float _projectileSpeed;
    private float _timeBetweenShots = 0.1f;
    private float _projectileDuration = 1;

    float nextTimeFire = 0;

    public SPShip origin;

    void Start()
    {
        LoadSettings();
    }

    void LoadSettings()
    {
        GameLoading loader = GameManager.Instance.GetComponent<GameLoading>();
        RemoteUnityManager unityRemote = GameManager.Instance.GetComponent<RemoteUnityManager>();

        if (!loader.loadingDone)
            unityRemote.onLaserDataUpdated += Initialize;
        else
        {
            Initialize(unityRemote.projectileSpeed, unityRemote.projectileDuration, unityRemote.minIntervalBetweenShots);
        }

    }

    public void Initialize(float projectileSpeed, float projectileDuration, float timeBetweenShots)
    {
       
        _projectileSpeed = projectileSpeed;
        _projectileDuration = projectileDuration;
        _timeBetweenShots = timeBetweenShots;
    }

    public void FireLaser(ref float energy)
    {
        GameObject projectile;
        if (!GameManager.Instance.offline)
        {
            projectile = Realtime.Instantiate(laserPref.name, laserOrigin.position, laserOrigin.rotation,
                ownedByClient: true, useInstance: _realtime);
        }
        else
        {
            projectile = Instantiate(laserPref, laserOrigin.position, laserOrigin.rotation);
        }

        Projectile proj = projectile.GetComponent<Projectile>();
        if (GameManager.Instance.offline)
        {
            proj.Initialize(_projectileDuration, origin);
        }
        else
        {
            proj.Initialize(_projectileDuration);

        }

        proj.Fire(laserOrigin.forward * _projectileSpeed);

        energy-=1;

        onPlaySound?.Invoke(0);
        //      PlaySound(0);

        nextTimeFire = Time.time + _timeBetweenShots;
        
    }

    public bool AllowToFire(float energy)
    {
        return ShotReloaded() && energy > 0;
    }

    public bool ShotReloaded()
    {
        return Time.time >= nextTimeFire;
    }
}
