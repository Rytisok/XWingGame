using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using TMPro;
using UnityEngine.UI;
using Unity.RemoteConfig;
public class Fly : Realtime
{
    public Transform x;

    public float currSpeed;
    float speedBoosted;
    float speedNormal;

    bool go = false;

    public Realtime _realtime;
    public Ship ship;

    [HideInInspector]
    public int energy;
    private int energyLimit = 20;
    private float timeBetweenEnergyRecovery = 0.15f;

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

    public Laser laser;
    public Missile missile;

    public TMP_Text debug;

    public Action<Vector3> onChangePos;
    public Action<Vector3> onChangeMousePos;

    private bool loaded;
    void Start()
    {
        LoadSettings();
    }

    void LoadSettings()
    {
        GameLoading loader = UnityRemoteManager.Instance.GetComponent<GameLoading>();
        UnityRemoteManager unityRemote = UnityRemoteManager.Instance;

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


    void UpdateEnergyData(int energyLimit,float timeBetweenEnergyRecovery,bool updateFromServer)
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
            if (Application.isEditor)
                MoveShipsEditor();

            Move();
        }
        //Missile
        if ((OVRInput.GetDown(OVRInput.RawButton.A) || Input.GetKeyDown(KeyCode.M)))
        {
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

    void Move()
    {
        Vector3 change = x.forward * Time.deltaTime * currSpeed;
        transform.position += change;
        onChangePos?.Invoke(change);
    }

    void MoveShipsEditor()
    {
        float mouseRatioX = Input.mousePosition.x / Screen.width;
        float mouseRatioY = Input.mousePosition.y / Screen.height;

        Vector3 mousePos = new Vector3(mouseRatioX - 0.5f, mouseRatioY - 0.5f, 0f);

        Vector3 curRot = transform.eulerAngles;
        curRot = new Vector3(curRot.x- mousePos.y, curRot.y + mousePos.x,curRot.z);

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(curRot), Time.deltaTime*100);
        onChangeMousePos?.Invoke(mousePos);
    }
    public void RestoreEnergy()
    {
        energy = energyLimit;
    }
}
