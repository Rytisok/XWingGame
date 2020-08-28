using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using UnityEngine.UI;


public class Ship : MonoBehaviour
{


    public GameObject explosion;
    public GameObject impact;
    public Realtime _realtime;
    public Transform[] restartPos;
    public bool alive = true;
    public ShipGlobal shp;
    public GameObject orbParticles;
    AudioSource aud;

    private const int startingHealth = 1;
    private int health = startingHealth;

    float prevHp = 1;

    public Text healthText;
    Fly fly;
    PlayerHealthScript trailScript;
    ScoreManager manager;
    PlayerScoreScript scoreScript;
    TSyncScript idScript;

    //visual stuff
    public GameObject asteroids;

    private void Start()
    {
        fly = GetComponentInParent<Fly>();
        aud = GetComponent<AudioSource>();
        asteroids.SetActive(false);
        manager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
    }

    void Update()
    {
        //check when your own instance is created
        if (shp == null)
        {
            FindInstance();
        }

        //get check health value from server
        if (trailScript != null)
        {
            health = trailScript.GetHealth();
        }

        if (health < prevHp)
        {
            MinHp();
        }

        prevHp = health;

        //update UI
        healthText.text = health.ToString();
    }

    void MinHp()
    {
        if (health <= 0)
        {
            if(alive)
            {
                Die();
            }
        }
        else
        {
            GameObject expl = Realtime.Instantiate(impact.name, transform.position, transform.rotation, ownedByClient: true, useInstance: _realtime);
            fly.PlaySound(1);
        }
    }

    public void PlusHp()
    {
        //touches orb
        fly.RestoreEnergy();
        aud.Play();
        GameObject part =Realtime.Instantiate(orbParticles.name, transform.position, transform.rotation, ownedByClient: false, useInstance: _realtime);
        Realtime.Destroy(part,3f);
    }

    void Restart()
    {
        //gameObject.SetActive(true);
        alive = true;
        shp.SetState(true);
        health = 1;
        healthText.text = health.ToString();
        if (trailScript != null)
        {
            trailScript.SetHealth(health);
        }
    }

    public void FindInstance()
    {
        int id = _realtime.clientID;
        
        foreach (GameObject plObj in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (plObj.GetComponent<RealtimeTransform>().ownerID == id)
            {
                shp = plObj.GetComponentInChildren<ShipGlobal>();

                trailScript = plObj.GetComponentInChildren<PlayerHealthScript>();
                idScript = plObj.GetComponentInChildren<TSyncScript>();
                scoreScript = plObj.GetComponent<PlayerScoreScript>();
                ShipInstance shipInstance = plObj.GetComponentInChildren<ShipInstance>();
                shipInstance.onOrbPickup += PlusHp;

                trailScript.SetHealth(health);
                scoreScript.SetDeaths(0);
                idScript.SetT(-1);

                EnterGame();
            }
        }
    }

    void Die()
    {
        //gets the ID of the projectile that hit the and adds the kill to the responsible player
        Debug.Log(idScript.GetT());
        if (idScript.GetT() != -1)
        {
            foreach (GameObject gm in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (gm.GetComponent<RealtimeView>().ownerID == idScript.GetT())
                {
                    gm.GetComponent<KillSyncScript>().SetKillCount(gm.GetComponent<KillSyncScript>().GetKillCount() + 1);
                    Debug.Log("SETTING KILLS");
                }
            }
        }

        scoreScript.SetDeaths(scoreScript.GetDeaths() + 1);

        GameObject expl = Realtime.Instantiate(explosion.name, transform.position, transform.rotation, ownedByClient: true, useInstance: _realtime);
        shp.SetState(false);
        transform.root.transform.position = restartPos[Random.Range(0, 3)].position;
        Invoke("Restart", 3.5f);
        fly.PlaySound(2);
        alive = false;
        //gameObject.SetActive(false);
    }

    void EnterGame()
    {
        asteroids.SetActive(true);
    }
}
