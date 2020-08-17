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
    int health = 1;
    public Text healthText;
    Fly fly;
    PlayerHealthScript trailScript;
    float prevHp = 1;
    private void Start()
    {
        fly = GetComponentInParent<Fly>();
        aud = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (_realtime.connected && shp == null)
        {
            FindInstance();
        }

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
        else
        {
            FindInstance();
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
                trailScript.SetHealth(health);
                //break;
            }
        }
    }

    void Die()
    {
        GameObject expl = Realtime.Instantiate(explosion.name, transform.position, transform.rotation, ownedByClient: true, useInstance: _realtime);
        shp.SetState(false);
        gameObject.SetActive(false);
        transform.root.transform.position = restartPos[Random.Range(0, 3)].position;
        Invoke("Restart", 3.5f);
        fly.PlaySound(2);
        alive = false;
    }

    
}
