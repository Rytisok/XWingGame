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

    #region Unity remote

    void Awake()
    {
        // Add a listener to apply settings when successfully retrieved: 
        ConfigManager.FetchCompleted += ApplyRemoteSettings;

        // Set the user’s unique ID:
        ConfigManager.SetCustomUserID("some-user-id");

        // Fetch configuration setting from the remote service: 
        ConfigManager.FetchConfigs<userAttributes, appAttributes>(new userAttributes(), new appAttributes());
    }

    void ApplyRemoteSettings(ConfigResponse configResponse)
    {
        // Conditionally update settings, depending on the response's origin:
        switch (configResponse.requestOrigin)
        {
            case ConfigOrigin.Default:
                Debug.Log("No settings loaded this session; using default values.");
                debug.text += "No settings loaded this session; using default values.\n";
                break;
            case ConfigOrigin.Cached:
                Debug.Log("No settings loaded this session; using cached values from a previous session.");
                debug.text += "No settings loaded this session; using cached values from a previous session.\n";

                break;
            case ConfigOrigin.Remote:
                Debug.Log("New settings loaded this session; update values accordingly.");
                debug.text += "New settings loaded this session; update values accordingly.\n";

                speedNormal = ConfigManager.appConfig.GetFloat("speed");
                speedBoosted = ConfigManager.appConfig.GetFloat("speedBoosted");
                projectileSpeed = ConfigManager.appConfig.GetFloat("projectileSpeed");
                timeBetweenShots = ConfigManager.appConfig.GetFloat("timeBetweenShots");
                projectileDuration = ConfigManager.appConfig.GetFloat("projectileDuration");
                energyLimit = ConfigManager.appConfig.GetInt("energyLimit");
                break;
        }
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //aud = GetComponent<AudioSource>();
        //audScrpt = GetComponent<PlayerAudioScript>();
        //   speedBoosted = speed * 2;
        energy = energyLimit;
    }

    // Update is called once per frame
    void Update()
    {
        if (ship.alive)
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
            nextTimeReload = Time.time + 0.15f;
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
        GameObject projectile = Instantiate(laser.name, tr.transform.position, tr.transform.rotation, ownedByClient: false, useInstance: _realtime);

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
