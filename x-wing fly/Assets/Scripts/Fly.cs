using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using UnityEngine.UI;

public class Fly : Realtime
{
    public Transform cntrl;
    public Transform x;
    public float speed;
    public float projectileSpeed;
    bool go = false;
    public GameObject laser;
    public Transform tr;
    public Realtime _realtime;
    public Ship ship;

    [HideInInspector]
    public int energy = 20;

    float nextTimeFire = 0;
    float nextTimeReload = 0;
    float nextTimeBoost = 0;
    float speedBoosted;
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

    // Start is called before the first frame update
    void Start()
    {
        //aud = GetComponent<AudioSource>();
        //audScrpt = GetComponent<PlayerAudioScript>();
        speedBoosted = speed * 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (audScrpt == null)
        {
            FindInstance();
        }

        if (ship.alive)
        {
            if ((OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger) || Input.GetKeyDown(KeyCode.W)))
            {
                //go = !go;
                go = true;
            }

            
            if (missileInControl == null)
            {
                if ((OVRInput.GetDown(OVRInput.RawButton.A)))
                {
                    LaunchMissile();
                }
            }
            else
            {
                float x = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x * 1.65f;
                float y = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y * 1.65f;

                missileInControl.transform.Rotate(-y, x, 0);
            }


            if ((OVRInput.Get(OVRInput.RawButton.RIndexTrigger) || Input.GetKeyDown(KeyCode.Space)) && Time.time >= nextTimeFire && energy > 0)
            {
                GameObject projectile = Realtime.Instantiate(laser.name, tr.transform.position, tr.transform.rotation, ownedByClient: true, useInstance: _realtime);
                projectile.GetComponent<Rigidbody>().velocity = x.forward * projectileSpeed;
                energy--;

                PlaySound(0);
                

                nextTimeFire = Time.time + 0.1f;
            }
            else if (energy < 20 && Time.time >= nextTimeReload && Time.time >= nextTimeFire && !(OVRInput.Get(OVRInput.RawButton.RHandTrigger)))
            {
                nextTimeReload = Time.time + 0.15f;
                energy++;
            }

            if (go)
            {
                transform.position += x.forward * Time.deltaTime * speed;
            }

            //boosting
            if ((OVRInput.Get(OVRInput.RawButton.RHandTrigger) || Input.GetKey(KeyCode.W)) && energy > 0)
            {
                boosting = true;
                thrusterAudio.pitch = 1.8f;
                foreach(ParticleSystemRenderer particle in particles)
                {
                    particle.material = boostThruster;
                    particle.GetComponent<ParticleSystem>().startSpeed = 0.2f;
                    particle.GetComponent<ParticleSystem>().startSize = 0.07f;
                }
                if(Time.time > nextTimeBoost)
                {
                    nextTimeBoost = Time.time + 0.3f;
                    energy--;
                }
            }
            else
            {
                if (boosting)
                {
                    EndBoost();
                }
                boosting = false;
            }

            if (boosting)
            {
                speed = Mathf.Lerp(speed, speedBoosted, Time.deltaTime * 20);
            }
            else if (speed != speedBoosted/2)
            {
                speed = Mathf.Lerp(speed, speedBoosted / 2, Time.deltaTime * 20);
            }
        }

        ammoCount.text = energy.ToString();
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
}
