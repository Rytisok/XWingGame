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
    public AudioSource aud;

    private const int startingHealth = 1;
    private int health = startingHealth;

    float prevHp = 1;

    public Text healthText;
    Fly fly;
    PlayerHealthScript trailScript;
    ScoreManager manager;
    PlayerScoreScript scoreScript;
    TSyncScript idScript;

    private bool instanceFound;
    public GameObject asteroids;
    public GameObject dome;

    public void Initialize(Fly fly, ScoreManager scoreManager)
    {
        this.fly = fly;
        instanceFound = false;
        asteroids.SetActive(false);
        manager = scoreManager;
    }

    void Update()
    {
        //check when your own instance is created
        if (shp == null)
        {
            FindInstance();
        }

        if (instanceFound)
        {
            //get check health value from server
            health = trailScript.GetHealth();

            if (health < prevHp)
            {
                MinHp();
            }
            else if (health > prevHp)
            {
                PlusHp();
            }

            prevHp = health;

            //update UI
            healthText.text = health.ToString();
        }
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

    void PlusHp()
    {
        //touches orb
        fly.RestoreEnergy();
        aud.Play();
        Realtime.Instantiate(orbParticles.name, transform.position, transform.rotation, ownedByClient: false, useInstance: _realtime);
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
            
        foreach (GameObject plObj in GameObject.FindGameObjectsWithTag("Ship"))
        {
            fly.debug.text = (plObj.GetComponent<RealtimeTransform>() == null).ToString();
           // fly.debug.text =(plObj.GetComponent<RealtimeTransform>().ownerID +"  /-/   " + id);
            if (plObj.GetComponent<RealtimeTransform>().ownerID == id)
            {
                shp = plObj.GetComponentInChildren<ShipGlobal>();

                trailScript = plObj.GetComponent<PlayerHealthScript>();
                idScript = plObj.GetComponent<TSyncScript>();
                scoreScript = plObj.GetComponent<PlayerScoreScript>();

                trailScript.SetHealth(health);
                //break;
                instanceFound = true;
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
        dome.SetActive(false);
        asteroids.SetActive(true);
    }
}
