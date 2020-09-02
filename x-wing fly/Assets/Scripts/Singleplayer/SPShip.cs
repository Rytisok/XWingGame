using System.Collections;
using System.Collections.Generic;
using Normal.Realtime;
using UnityEngine;
using UnityEngine.UI;

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

    float prevHp = 1;

    private SPPlayerHealth playerHealth;
    private SPPlayerScore playerScore;

    public Text healthText;
    private Fly fly;
    private ScoreManager manager;

    //visual stuff
    public GameObject[] shipModels;
    public int team = -1;
    public bool isBot= false;

    private void Start()
    {
        fly = GetComponentInParent<Fly>();
        aud = GetComponent<AudioSource>();
        playerHealth = GetComponent<SPPlayerHealth>();
        playerScore = GetComponent<SPPlayerScore>();

        // manager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
    }

    void Update()
    {
        //get check health value from server
        if (playerHealth != null)
        {
            health = playerHealth.currentHealth;
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
        fly.PlaySound(1);
        Destroy(expl,5f);
    }

    void Explode()
    {
        GameObject expl = Instantiate(explosion, transform.position, transform.rotation);
        fly.PlaySound(2);
        Destroy(expl, 5f);
    }

    public void PlusHp()
    {
        //touches orb
        fly.RestoreEnergy();
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
        transform.root.transform.position = restartPos[Random.Range(0, 3)].position;
        Invoke("Restart", 3.5f);
        fly.PlaySound(2);
        alive = false;
        //gameObject.SetActive(false);
    }

    void Restart()
    {
        //gameObject.SetActive(true);
        alive = true;
        SetState(true);

        playerHealth.ResetValues();
        healthText.text = playerHealth.currentHealth.ToString();

    }

    public void SelectShip(int n)
    {
        shipModels[0].SetActive(false);
        shipModels[1].SetActive(false);

        shipModels[n].SetActive(true);
        team = n;

     /*   if (!GameManager.offline)
        {
           // shp.GetComponentInParent<TeamSync>().SetTeam(n);
        }*/

        manager.SetScoreText();

        Laser laser = GetComponentInParent<Laser>();
        laser.laserPref = laser.laserVariants[n];
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

                break;

            //other player
            case 9:
                playerHealth.ChangeHealth(-playerHealth.currentHealth);

                break;

            //asteroid
            case 11:
                playerHealth.ChangeHealth(-playerHealth.currentHealth);

                break;

            //rocket explosion
            case 12:
                playerHealth.ChangeHealth(-1);

                //set the owner ID of the projectile, that hit
              //  idModel.SetT(other.GetComponent<RealtimeView>().ownerID);
                break;

            //rocket direct hit
            case 13: //NO SCRIPT TO DETERMINE ORIGIN
                playerHealth.ChangeHealth(-playerHealth.currentHealth);
              /*  if (playerHealth.currentHealth == 0)
                    other.GetComponent<Projectile>().origin.CreditKill();*/
                break;

            //orb
            case 14:
                if (!playerHealth.IsAtMaxHP())
                {
                    playerHealth.ChangeHealth(1);
                }

                PlusHp();

                Realtime.Destroy(other.gameObject);
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
