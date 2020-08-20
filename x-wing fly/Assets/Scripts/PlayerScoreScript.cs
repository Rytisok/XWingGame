using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class PlayerScoreScript : RealtimeComponent
{
    private PlayerScoreModel _model;

    [SerializeField]
    private int kills = 0;
    private int deaths = 0;
    private ScoreManager manager;
    [SerializeField]
    private int connectTime = 0;

    private void Awake()
    {
        manager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        
    }

    private void Update()
    {
        if (connectTime == 0)
        {
            connectTime = (int)GetComponent<RealtimeView>().realtime.room.time;
        }
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

        if (GetComponent<RealtimeView>().isOwnedLocally)
        {
            manager.SetScoreText();
        }
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
    public int GetConnectTime()
    {
        return connectTime;
    }

}
