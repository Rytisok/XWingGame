using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Normal.Realtime;
using Normal.Realtime.Serialization;
using UnityEngine;

public class BotsSpawner : MonoBehaviour
{
    private List<GameObject> bots;

    private const float refreshTime = 5f;
    private float t;
    private const int botsCount = 1;
    public Realtime _realtime;
    public GameObject botPref;
    // Start is called before the first frame update
    void Start()
    {
        t = 0;
    }


    void FindAllBots()
    {
         bots = GameObject.FindGameObjectsWithTag("Bot").ToList();
      
    }

    // Update is called once per frame
    void Update()
    {
     
        if (t >= refreshTime)
        {
            StartCoroutine(RefreshBots());
            t = 0;
        }

        t += Time.deltaTime;
    }

    IEnumerator RefreshBots()
    {
        FindAllBots();
        while (bots.Count < botsCount)
        {
            SpawnBot();
            yield return new WaitForSeconds(1f);
        }
    }

    void SpawnBot()
    {
         GameObject bot = Realtime.Instantiate(botPref.name, Vector3.zero, Quaternion.identity,
            true,  false, false,_realtime);
         bot.GetComponent<Laser>()._realtime = _realtime;
         bots.Add(bot);
       // bot.transform.parent = transform;
    }
}
