using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Normal.Realtime;
using Normal.Realtime.Serialization;
using UnityEngine;

public class BotsSpawner : MonoBehaviour
{
    public SPScoreManager scoreManager;
    public Transform[] restartPos;
    private List<GameObject> bots;

    private const float refreshTime = 1f;
    private float t;
    private const int botsCount = 1;
    public GameObject botPref;

    private bool loaded;
    // Start is called before the first frame update
    void Start()
    {
        bots = new List<GameObject>();
        t = 0;

        LoadSettings();
    }

    void LoadSettings()
    {
        GameLoading loader = GameManager.Instance.GetComponent<GameLoading>();

        if (!loader.loadingDone)
        {
            loader.onLoadingDone += () =>
            {
                loaded = true;
            };
        }
        else
        {
            loaded = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (loaded)
        {

            if (t >= refreshTime)
            {
                StartCoroutine(RefreshBots());
                t = 0;
            }

            t += Time.deltaTime;
        }
    }

    IEnumerator RefreshBots()
    {
        while (bots.Count < botsCount)
        {
            SpawnBot();
            yield return new WaitForSeconds(1f);
        }
    }

    void SpawnBot()
    {
         GameObject bot = Instantiate(botPref, Vector3.zero, Quaternion.identity);

         SPShip botShip = bot.GetComponent<SPShip>();
         botShip.SetupBot(scoreManager, restartPos);

        bots.Add(bot);
       // bot.transform.parent = transform;
    }
}
