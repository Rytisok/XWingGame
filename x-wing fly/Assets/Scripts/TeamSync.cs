using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class TeamSync : RealtimeComponent
{
    private TeamModel _model;
    private int team;

    private TeamModel model
    {
        set
        {
            if (_model != null)
            {
                _model.teamDidChange -= TeamChanged;
            }

            _model = value;

            if (_model != null)
            {
                UpdateValue();
                _model.teamDidChange += TeamChanged;
            }
        }
    }
    void TeamChanged(TeamModel model, int value)
    {
        team = _model.team;
    }
    void UpdateValue()
    {
        team = _model.team;
    }
    public int GetTeam()
    {
        return team;
    }
    public void SetTeam(int t)
    {
        _model.team = t;
    }
}
