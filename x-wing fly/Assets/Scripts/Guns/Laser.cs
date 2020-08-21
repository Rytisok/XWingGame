using System.Collections;
using System.Collections.Generic;
using Normal.Realtime;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Realtime _realtime;
    public GameObject laserPref;
    public Transform laserOrigin;

    private float _projectileSpeed;
    private float _timeBetweenShots = 0.1f;
    private float _projectileDuration = 1;

    float nextTimeFire = 0;

    private bool _initializedFromServer;

    void Awake()
    {
        UnityRemoteManager.Instance.onLaserDataUpdated += Initialize;
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
        GameObject projectile = Realtime.Instantiate(laserPref.name, laserOrigin.position, laserOrigin.rotation,
            ownedByClient: true, useInstance: _realtime);

        Projectile proj = projectile.GetComponent<Projectile>();
        proj.Initialize(_projectileDuration);
        proj.Fire(transform.forward * _projectileSpeed);

        energy--;

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
