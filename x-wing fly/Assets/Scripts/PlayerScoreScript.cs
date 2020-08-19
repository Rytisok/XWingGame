using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class PlayerScoreScript : RealtimeComponent
{
    private PlayerScoreModel _model;

    public int kills = 0;
    public int deaths = 0;
    ScoreManager manager;

    private void Awake()
    {
        manager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
    }



    private PlayerScoreModel model
    {
        set
        {
            if (_model != null)
            {
                _model.killsDidChange -= KillsChanged;
                _model.deathsDidChange -= DeathsChanged;
                //_model.te
            }

            _model = value;

            if (_model != null)
            {
                UpdateValues();
                _model.killsDidChange += KillsChanged;
                _model.deathsDidChange += DeathsChanged;
            }
        }
    }

    private void KillsChanged(PlayerScoreModel model, int value)
    {
        UpdateValues();
    }
    private void DeathsChanged(PlayerScoreModel model, int value)
    {
        UpdateValues();
    }

    private void UpdateValues()
    {
        kills = _model.kills;
        deaths = _model.deaths;
    }

    public int GetKills()
    {
        return kills;
    }
    public int GetDeaths()
    {
        return deaths;
    }

    public void SetKills(int val)
    {
        _model.kills = val;
    }
    public void SetDeaths(int val)
    {
        _model.deaths = val;
    }
}
