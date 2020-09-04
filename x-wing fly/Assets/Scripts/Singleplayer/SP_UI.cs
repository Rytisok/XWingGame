using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SP_UI : MonoBehaviour
{
    public GameObject statsBoard;

    public GameObject statsPref;
    public GameObject statsParentPref;
    private Vector3 statsParentPos;
    public GameObject statsParent;

    private bool statsShown;
    // Start is called before the first frame update
    void Start()
    {
        statsShown = false;
        StatsState(statsShown);
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetKeyDown(KeyCode.Space))
        {
            statsShown = !statsShown;

            Debug.Log(statsShown);
            StatsState(statsShown);
        }
    }

    void StatsState(bool state)
    {
        statsBoard.SetActive(state);
    }

    public void UpdateStats(List<PlayerStats> stats)
    {

        if (statsParent != null)
        {
            statsParentPos = statsParent.transform.position;
            Destroy(statsParent);
            statsParent = Instantiate(statsParentPref, statsParentPos, Quaternion.identity, statsBoard.transform);
        }

        for (int i = 0; i < stats.Count; i++)
        {
            GameObject obj = Instantiate(statsPref, Vector3.zero, Quaternion.identity, statsParent.transform);
            obj.GetComponent<TMP_Text>().SetText(stats[i].ID+" | "+stats[i].k +" | "+stats[i].d +" |");

        }
    }
}
