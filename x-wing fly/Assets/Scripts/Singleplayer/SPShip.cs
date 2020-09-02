using System;
using System.Collections;
using System.Collections.Generic;
using Normal.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SPPlayerHealth))]
[RequireComponent(typeof(SPPlayerScore))]
[RequireComponent(typeof(AudioSource))]

public class SPShip : MonoBehaviour
{
    public GameObject explosion;
    public GameObject impact;
    public GameObject orbParticles;

    public Transform[] restartPos;
    public bool alive = true;
    private AudioSource aud;

    private const int startingHealth = 1;
    private int health = startingHealth;

    private SPPlayerHealth playerHealth;
    private SPPlayerScore playerScore;
    public SPScoreManager manager;

    public Text healthText;
    private SPAudioController audioManager;

    public int team = -1;
    public bool isBot= false;
    public Action onOrbPickup;
    private void Start()
    {
        Setup();
    }

    void Setup()
    {
        audioManager = GetComponentInParent<SPAudioController>();
        aud = GetComponent<AudioSource>();
        playerHealth = GetComponent<SPPlayerHealth>();
        playerScore = GetComponent<SPPlayerScore>();
        if (!isBot)
            playerScore.ResetValues();
    }

    public void SetupBot(SPScoreManager scoreManager, Transform[] restartPos)
    {
        Setup();
        this.restartPos = restartPos;
        manager = scoreManager;
        playerScore.scoreManager = scoreManager;
        playerScore.ResetValues();
    }

    void Update()
    {
        healthText.text = health.ToString();
    }

    void MinHp()
    {
        if (playerHealth.currentHealth <= 0)
        {
            if (alive)
            {
                Die();
            }
        }
        else
        {
            Hit();
        }
    }

    void Hit()
    {
        GameObject expl = Instantiate(impact, transform.position, transform.rotation);
        audioManager.PlayAudio(1);
        Destroy(expl,5f);
    }

    void Explode()
    {
        GameObject expl = Instantiate(explosion, transform.position, transform.rotation);
        audioManager.PlayAudio(2);
        Destroy(expl, 5f);
    }

    public void PlusHp()
    {
        //touches orb
        aud.Play();
        GameObject part = Instantiate(orbParticles, transform.position, transform.rotation);
        Destroy(part, 3f);
    }

    public void CreditKill()
    {
        playerScore.ChangeKills(1);
    }

    void Die()
    {

        playerScore.ChangeDeaths(1);
        Explode();

        SetState(false);
        transform.root.position = restartPos[Random.Range(0, 3)].position;
        Invoke("Restart", 3.5f);
        audioManager.PlayAudio(2);
        alive = false;
    }

    void Restart()
    {
        alive = true;
        SetState(true);

        playerHealth.ResetValues();
        healthText.text = playerHealth.currentHealth.ToString();

    }


    private void OnTriggerEnter(Collider other)
    {
       
        switch (other.gameObject.layer)
        {
            //laser
            case 8:
                playerHealth.ChangeHealth(-1);
                //set the owner ID of the projectile, that hit
                if(playerHealth.currentHealth == 0)
                    other.GetComponent<Projectile>().origin.CreditKill();
                MinHp();

                break;

            //other player
            case 9:
                playerHealth.ChangeHealth(-playerHealth.currentHealth);
                MinHp();

                break;

            //asteroid
            case 11:
                playerHealth.ChangeHealth(-playerHealth.currentHealth);
                MinHp();

                break;

            //rocket explosion
            case 12:
                playerHealth.ChangeHealth(-1);
                MinHp();

                //set the owner ID of the projectile, that hit
                //  idModel.SetT(other.GetComponent<RealtimeView>().ownerID);
                break;

            //rocket direct hit
            case 13: //NO SCRIPT TO DETERMINE ORIGIN
                playerHealth.ChangeHealth(-playerHealth.currentHealth);
                MinHp();

                /*  if (playerHealth.currentHealth == 0)
                      other.GetComponent<Projectile>().origin.CreditKill();*/
                break;

            //orb
            case 14:
                if (!playerHealth.IsAtMaxHP())
                {
                    playerHealth.ChangeHealth(1);
                }

                onOrbPickup?.Invoke();
                PlusHp();

                Destroy(other.gameObject);
                break;
        }
        
    }
    public void SetState(bool alive)
    {
        var renderers = transform.root.GetComponentsInChildren<MeshRenderer>();
        foreach (var renderer in renderers)
        {
            renderer.enabled = alive;
        }
    }
}
