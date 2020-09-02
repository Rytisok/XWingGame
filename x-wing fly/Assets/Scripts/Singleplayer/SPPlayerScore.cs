using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPPlayerScore : MonoBehaviour
{
    public int kills { get; private set; }
    public int deaths { get; private set; }
    
    public SPScoreManager scoreManager;

    public void ResetValues()
    {
        kills = 0;
        deaths = 0;
        UpdateValues();
    }

    private void UpdateValues()
    {
        scoreManager.SetScoreText();
    }

    public void ChangeKills(int change)
    {
        kills += change;
        UpdateValues();
    }
    public void ChangeDeaths(int change)
    {
        deaths += change;
        UpdateValues();

    }

}
