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

    private bool _initializedFromServer;

    void Start()
    {
        LoadSettings();
    }

    void LoadSettings()
    {
        GameLoading loader = RemoteUnityManager.Instance.GetComponent<GameLoading>();
        RemoteUnityManager unityRemote = RemoteUnityManager.Instance;
        
        if (!loader.loadingDone)
            unityRemote.onLaserDataUpdated += Initialize;
        else
        {
            Initialize(unityRemote.projectileSpeed, unityRemote.projectileDuration, unityRemote.timeBetweenShots, true);
        }

    }

    public void Initialize(float projectileSpeed, float projectileDuration, float timeBetweenShots, bool initializedFromServer)
    {
        if (!_initializedFromServer)
        {
            _projectileSpeed = projectileSpeed;
            _projectileDuration = projectileDuration;
            _timeBetweenShots = timeBetweenShots;
        }

        _initializedFromServer = initializedFromServer;
    }

    public void FireLaser(ref int energy)
    {
        if (laserPref == null)
        {
            if (_realtime.GetCurrentRoomName() == "t" && _realtime.clientID % 2 != 0)
            {
                laserPref = laserVariants[1];
            }
            else
            {
                laserPref = laserVariants[0];
            }
        }

        GameObject projectile = Realtime.Instantiate(laserPref.name, laserOrigin.position, laserOrigin.rotation,
            ownedByClient: true, useInstance: _realtime);

        Projectile proj = projectile.GetComponent<Projectile>();
        proj.Initialize(_projectileDuration);
        proj.Fire(laserOrigin.forward * _projectileSpeed);

        energy--;

        onPlaySound?.Invoke(0);
        //      PlaySound(0);

        nextTimeFire = Time.time + _timeBetweenShots;
        
    }

    public bool AllowToFire(int energy)
    {
        return ShotReloaded() && energy > 0;
    }

    public bool ShotReloaded()
    {
        return Time.time >= nextTimeFire;
    }
}
