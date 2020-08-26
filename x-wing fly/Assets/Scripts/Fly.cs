using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using TMPro;
using UnityEngine.UI;
using Unity.RemoteConfig;
public class Fly : Realtime
{
    public struct userAttributes
    {
    }

    public struct appAttributes
    {
    }

    public Transform cntrl;
    public Transform x;

    public float currSpeed;
    float speedBoosted;
    float speedNormal;

    public float projectileSpeed;
    private float timeBetweenShots =0.1f;
    private float projectileDuration = 1;

    bool go = false;
    public GameObject laser;
    public Transform tr;
    public Realtime _realtime;
    public Ship ship;

    [HideInInspector]
    public int energy;
    private int energyLimit = 20;
    private float timeBetweenEnergyRecovery = 0.15f;

    float nextTimeFire = 0;
    float nextTimeReload = 0;
    float nextTimeBoost = 0;
    public Text ammoCount;
    bool boosting = false;
    public Material thruster;
    public Material boostThruster;
    public ParticleSystemRenderer[] particles;
    public PlayerAudioScript audScrpt;
    bool switcheroo = false;
    public AudioSource thrusterAudio;

    public GameObject rocket;
    GameObject missileInControl;

    public TMP_Text debug;

    private bool loaded;
    void Start()
    {
        energy = energyLimit;

        LoadSettings();
    }

    void LoadSettings()
    {
        GameLoading loader = RemoteUnityManager.Instance.GetComponent<GameLoading>();
        RemoteUnityManager remoteUnity = RemoteUnityManager.Instance;

        if (!loader.loadingDone)
        {
            remoteUnity.onEnergyDataUpdated += UpdateEnergyData;
            remoteUnity.onSpeedDataUpdated += UpdateSpeedData;
            loader.onLoadingDone += () =>
            {
                loaded = true;
            };
        }
        else
        {
            UpdateEnergyData(remoteUnity.energyLimit, remoteUnity.timeBetweenEnergyRecovery, true);
            UpdateSpeedData(remoteUnity.speedNormal, remoteUnity.speedBoosted, true);
            loaded = true;
        }

    }
    void UpdateEnergyData(int energyLimit, float timeBetweenEnergyRecovery, bool updateFromServer)
    {
        this.energyLimit = energyLimit;
        this.timeBetweenEnergyRecovery = timeBetweenEnergyRecovery;

        energy = energyLimit;

    }
    void UpdateSpeedData(float speedNormal, float speedBoosted, bool updateFromServer)
    {
        this.speedNormal = speedNormal;
        this.speedBoosted = speedBoosted;
    }


    // Update is called once per frame
    void Update()
    {
        if (ship.alive && loaded)
        {
            Controls();
        }

        ammoCount.text = energy.ToString();
    }

    void Controls()
    {
        if ((OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger) || Input.GetKeyDown(KeyCode.W)))
        {
            go = true;
        }
        if (go)
        {
            Move();
        }
        //Missile
        if ((OVRInput.GetDown(OVRInput.RawButton.A)))
        {
            if (missileInControl == null)
            {
                LaunchMissile();
            }
        }
        if (missileInControl != null)
        {
            MissileControler();
        }
        //--------------
        //Laser
        if ((OVRInput.Get(OVRInput.RawButton.RIndexTrigger) || Input.GetKeyDown(KeyCode.Space)) && Time.time >= nextTimeFire && energy > 0)
        {
            FireLaser();
        }
        else if (energy < energyLimit && Time.time >= nextTimeReload && Time.time >= nextTimeFire && !(OVRInput.Get(OVRInput.RawButton.RHandTrigger)))
        {
            nextTimeReload = Time.time + timeBetweenEnergyRecovery;
            energy++;
        }
        //--------------

        //boosting
        if ((OVRInput.Get(OVRInput.RawButton.RHandTrigger) || Input.GetKey(KeyCode.W)) && energy > 0)
        {
            if (!boosting)
            {
                StartBoost();
            }
        }
        else
        {
            if (boosting)
            {
                EndBoost();
            }
        }

        if (boosting)
        {
            if (Time.time > nextTimeBoost)
            {
                nextTimeBoost = Time.time + 0.3f;
                energy--;
            }

            currSpeed = Mathf.Lerp(currSpeed, speedBoosted, Time.deltaTime * 20);
        }
        else
        {
            if (currSpeed != speedNormal)
            {
                currSpeed = Mathf.Lerp(currSpeed, speedNormal, Time.deltaTime * 20);
            }
        }
        //--------------

    }

    void FindInstance()
    {
        int id = _realtime.clientID;
        foreach (GameObject pl in GameObject.FindGameObjectsWithTag("Player"))
         {
             if(pl.GetComponent<RealtimeView>().ownerID == id)
             {
                audScrpt = pl.GetComponent<PlayerAudioScript>();
             }
         }
        Debug.Log("searching");
    }

    void EndBoost()
    {
        foreach (ParticleSystemRenderer particle in particles)
        {
            particle.material = thruster;
            particle.GetComponent<ParticleSystem>().startSpeed = 0.1f;
            particle.GetComponent<ParticleSystem>().startSize = 0.04f;

        }
        thrusterAudio.pitch = 0.8f;
        boosting = false;
    }
    void StartBoost()
    {
        thrusterAudio.pitch = 1.8f;
        foreach (ParticleSystemRenderer particle in particles)
        {
            particle.material = boostThruster;
            particle.GetComponent<ParticleSystem>().startSpeed = 0.2f;
            particle.GetComponent<ParticleSystem>().startSize = 0.07f;
        }

        boosting = true;

    }

    public void PlaySound(int num)
    {
        if (audScrpt != null)
        {
            audScrpt.PlaySound(num);
            audScrpt.PlaySound(-1);
        }
        else
        {
            FindInstance();
        }
    }

    void LaunchMissile()
    {
        GameObject missile = Realtime.Instantiate(rocket.name, tr.transform.position, tr.transform.rotation, ownedByClient: true, useInstance: _realtime);
        missileInControl = missile;
    }
    void MissileControler()
    {
        float x = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x * 1.65f;
        float y = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y * 1.65f;

        missileInControl.transform.Rotate(-y, x, 0);
    }
    void FireLaser()
    {
        GameObject projectile = Instantiate(laser.name, tr.transform.position, tr.transform.rotation, ownedByClient: true, useInstance: _realtime);

        Projectile proj = projectile.GetComponent<Projectile>();
        proj.Initialize(projectileDuration);
        proj.Fire(x.forward * projectileSpeed);

        energy--;

        PlaySound(0);


        nextTimeFire = Time.time + timeBetweenShots;
    }

    void Move()
    {
        transform.position += x.forward * Time.deltaTime * currSpeed;

    }
    public void RestoreEnergy()
    {
        energy = energyLimit;
    }
}
