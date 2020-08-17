using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using UnityEngine.UI;

public class PlayerHealthScript : RealtimeComponent
{
    private TrailModel _model;
    private TrailRenderer trail;

    private void Awake()
    {
        trail = GetComponent<TrailRenderer>();
    }

    private TrailModel model
    {
        set
        {
            if (_model != null)
            {
                _model.healthDidChange -= HealthDidChange;
            }

            _model = value;

            if (_model != null)
            {
                UpdateDisplay();
                _model.healthDidChange += HealthDidChange;
            }
        }
    }

    private void HealthDidChange(TrailModel model, int value)
    {
        UpdateDisplay();
    }
    private void UpdateDisplay()
    {
        trail.time = _model.health * 0.2f;
    }

    public int GetHealth()
    {
        return _model.health;
    }

    public void SetHealth(int h)
    {
        _model.health = h;
    }
}
