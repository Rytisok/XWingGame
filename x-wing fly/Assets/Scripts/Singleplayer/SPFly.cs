using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SPAudioController))]
public class SPFly : MonoBehaviour
{
    public Transform x;

    public float currSpeed;
    float speedBoosted;
    float speedNormal;

    bool go = false;

    public SPShip ship;

    [HideInInspector]
    public int energy;
    private int energyLimit = 20;
    private float timeBetweenEnergyRecovery = 0.15f;

    float nextTimeReload = 0;
    float nextTimeBoost = 0;
    bool boosting = false;
    public Material thruster;
    public Material boostThruster;
    public ParticleSystemRenderer[] particles;
    bool switcheroo = false;
    public AudioSource thrusterAudio;

    public Laser laser;
    public Missile missile;

    public Text ammoCount;
    public TMP_Text debug;

    private bool loaded;
    private int missileCount = 1;
    void Start()
    {

        ship.onOrbPickup += RestoreEnergy;

        energy = energyLimit;
        laser.onPlaySound += GetComponent<SPAudioController>().PlayAudio;
        LoadSettings();
    }

    void LoadSettings()
    {
        GameLoading loader = GameManager.Instance.GetComponent<GameLoading>();
        RemoteUnityManager unityRemote = GameManager.Instance.GetComponent<RemoteUnityManager>();

        if (!loader.loadingDone)
        {
            unityRemote.onEnergyDataUpdated += UpdateEnergyData;
            unityRemote.onSpeedDataUpdated += UpdateSpeedData;
            loader.onLoadingDone += () =>
            {
                loaded = true;
            };
        }
        else
        {
            UpdateEnergyData(unityRemote.energyLimit, unityRemote.timeBetweenEnergyRecovery, true);
            UpdateSpeedData(unityRemote.speedNormal, unityRemote.speedBoosted, true);
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

        debug.text = (ship.alive && loaded).ToString();
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
        if ((OVRInput.GetDown(OVRInput.RawButton.A) || Input.GetKeyDown(KeyCode.M)) && missileCount > 0)
        {
            missileCount--;
            missile.LaunchMissile();
        }
        //--------------
        //Laser
        if ((OVRInput.Get(OVRInput.RawButton.RIndexTrigger) || Input.GetMouseButton(0)) && laser.AllowToFire(energy))
        {
            laser.FireLaser(ref energy);
        }
        else if (energy < energyLimit && Time.time >= nextTimeReload && laser.ShotReloaded() && !(OVRInput.Get(OVRInput.RawButton.RHandTrigger)))
        {
            nextTimeReload = Time.time + timeBetweenEnergyRecovery;
            energy++;
        }
        //--------------

        //boosting
        if ((OVRInput.Get(OVRInput.RawButton.RHandTrigger) || Input.GetMouseButton(1)) && energy > 0)
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

    void Move()
    {
        transform.position += x.forward * Time.deltaTime * currSpeed;

    }
    public void RestoreEnergy()
    {
        energy = energyLimit;
        missileCount = 1;
    }
}
