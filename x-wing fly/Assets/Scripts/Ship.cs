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
    ScoreManager manager;
    int health = 1;
    public Text healthText;
    Fly fly;
    PlayerHealthScript trailScript;
    PlayerScoreScript scoreScript;
    TSyncScript idScript;
    float prevHp = 1;

    public GameObject dome;
    public GameObject asteroids;

    public int kills = 0;
    public int deaths = 0;


    private void Awake()
    {
        fly = GetComponentInParent<Fly>();
        aud = GetComponent<AudioSource>();
        GetComponentInChildren<MeshRenderer>().enabled = false;
        asteroids.SetActive(false);
        manager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        Debug.Log("awaking");
    }

    void Update()
    {
        //check when your own instance is created
        if (shp == null)
        {
            FindInstance();
        }

        //get health value from server
        if (trailScript != null)
        {
            health = trailScript.GetHealth();
        }
        

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
        if (scoreScript != null)
        {
            //kills = scoreScript.GetKills();
            //deaths = scoreScript.GetDeaths();
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
        fly.energy = 20;
        aud.Play();
        Realtime.Instantiate(orbParticles.name, transform.position, transform.rotation, ownedByClient: false, useInstance: _realtime);
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.layer == 11 || other.gameObject.layer == 9)
        {
            if (other.GetComponentInParent<RealtimeTransform>() != null)
            {
                if (other.GetComponentInParent<RealtimeTransform>().isOwnedLocally)
                {
                    other.GetComponent<BoxCollider>().enabled = false;
                }
                else
                {
                    health = 0;
                    Die();
                }
            }
            else
            {
                health = 0;
                Die();
            }
        }
        if (other.gameObject.layer == 12)
        {
            if (health > 4)
            {
                health -= 4;
            }
            else
            {
                health = 0;
                Die();
            }
        }
        if (other.gameObject.layer == 13)
        {
            health = 0;
            Die();
        }

        if (trailScript != null)
        {
            trailScript.SetHealth(health);
        }
        else
        {
            FindInstance();
        }
    }
    */
    void Restart()
    {
        gameObject.SetActive(true);
        
        alive = true;
        shp.SetState(true);
        health = 1;
        healthText.text = health.ToString();
        if (trailScript != null)
        {
            trailScript.SetHealth(health);
        }
        //retarded
      /*  scoreScript.SetKills(kills);
        scoreScript.SetDeaths(deaths);
        manager.SetScoreText();*/
    }

    public void FindInstance()
    {
        Debug.Log("finding instance");
        int id = _realtime.clientID;
        
        foreach (GameObject plObj in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (plObj.GetComponent<RealtimeTransform>().ownerID == id)
            {
                //instance is found
                shp = plObj.GetComponentInChildren<ShipGlobal>();

                trailScript = plObj.GetComponentInChildren<PlayerHealthScript>();
                idScript = plObj.GetComponentInChildren<TSyncScript>();
                scoreScript = plObj.GetComponent<PlayerScoreScript>();

                trailScript.SetHealth(health);
                scoreScript.SetDeaths(0);
                //scoreScript.SetKills(0);

                EnterGame();
            }
        }
    }

    void Die()
    {
        //deaths++;

        if (idScript.GetT() != -1)
        {
            foreach(GameObject gm in  GameObject.FindGameObjectsWithTag("Player"))
            {
                if(gm.GetComponent<RealtimeView>().ownerID == idScript.GetT())
                {
                    gm.GetComponent<KillSyncScript>().SetKillCount(gm.GetComponent<KillSyncScript>().GetKillCount() + 1);
                    Debug.Log("SETTING KILLS");
                }
            }
            idScript.SetT(-1);
        }

        scoreScript.SetDeaths(scoreScript.GetDeaths() + 1);
        //scoreScript.SetKills(scoreScript.GetKills());
        GameObject expl = Realtime.Instantiate(explosion.name, transform.position, transform.rotation, ownedByClient: true, useInstance: _realtime);
        shp.SetState(false);
        transform.root.transform.position = restartPos[Random.Range(0, 3)].position;
        Invoke("Restart", 3.5f);
        fly.PlaySound(2);
        //manager.SetScoreText();
        alive = false;
        gameObject.SetActive(false);
    }

    void EnterGame()
    {
        GetComponentInChildren<MeshRenderer>().enabled = false;
        dome.SetActive(false);
        asteroids.SetActive(true);
    }
    
}
