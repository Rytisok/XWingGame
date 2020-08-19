using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class KillSyncScript : RealtimeComponent
{
    private KillsModel _model;
    public int kills;
    private ScoreManager manager;
    private void Awake()
    {
        manager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();

    }

    private KillsModel model
    {
        set
        {
            if (_model != null)
            {
                _model.killCountDidChange -= TDidChange;
            }

            _model = value;

            if (_model != null)
            {
                UpdateValue();
                _model.killCountDidChange += TDidChange;
            }
        }
    }

    void TDidChange(KillsModel model, int value)
    {
        UpdateValue();
    }
    void UpdateValue()
    {
        kills = _model.killCount;
    }

    public void SetKillCount(int k)
    {
        _model.killCount = k;
    }

    public int GetKillCount()
    {
        return _model.killCount;
    }
}
