using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
public class TSyncScript : RealtimeComponent
{
    private Tm _model;
    public int h;


    private void Awake()
    {
        //SetT(0);
    }

    private void Update()
    {
       
    }

    private Tm model
    {
        set
        {
            if (_model != null)
            {
                _model.tDidChange -= TDidChange;
            }

            _model = value;

            if (_model != null)
            {
                UpdateValue();
                _model.tDidChange += TDidChange;
            }
        }
    }

    void TDidChange(Tm model, int value)
    {
        UpdateValue();
    }
    void UpdateValue()
    {
        h = _model.t;
    }

    public void SetT(int l)
    {
        _model.t = l;
    }

    public int GetT()
    {
        return _model.t;
    }
}
