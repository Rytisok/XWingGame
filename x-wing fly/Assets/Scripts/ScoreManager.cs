using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using TMPro;

public class ScoreManager : RealtimeComponent
{

    private ScoreBoardModel _model;
    public RealtimeAvatarManager _avatarManager;
    public TMP_Text text;
    public Realtime _realtime;
    double nxtCheck = 0;

    private void OnEnable()
    {
        _avatarManager.avatarCreated += AvatarChangedUpdateScore;
        _avatarManager.avatarDestroyed += AvatarChangedUpdateScore;
        LoadSettings();
    }

    void LoadSettings()
    {
        GameLoading loader = UnityRemoteManager.Instance.GetComponent<GameLoading>();

        if (!loader.loadingDone)
            loader.onLoadingDone += () =>
            {
                nxtCheck = GetComponent<RealtimeView>().realtime.room.time;
            };
        else
        {
            nxtCheck = GetComponent<RealtimeView>().realtime.room.time;
        }

    }

    private void AvatarChangedUpdateScore(RealtimeAvatarManager avatarManager, RealtimeAvatar avatar, bool isLocalAvatar)
    {
        SetScoreText();
    }

    private void Update()
    {
       /* if (Time.time > nxtCheck)
        {
            SetScoreText();
            nxtCheck = Time.time+ 3;
            Debug.Log("update");
        }*/
    }

    private ScoreBoardModel model
    {
        set
        {
            if (_model != null)
            {
                _model.scoreTextDidChange -= ScoreDidChange;
            }

            _model = value;

            if (_model != null)
            {
                UpdateScore();
                _model.scoreTextDidChange += ScoreDidChange;
            }
        }
    }

    private void ScoreDidChange(ScoreBoardModel model, string value)
    {
        UpdateScore();
    }

    private void UpdateScore()
    {
        text.text = _model.scoreText;
    }

    public void SetScoreText()
    {
        int playerID = 0;
        
        string t = "";
        foreach(var player in _avatarManager.avatars)
        {
            playerID = player.Key + 1;
            Debug.Log(player.Key);
            GameObject pl = _avatarManager.avatars[player.Key].gameObject;
            t += "Player " + playerID + "    " + pl.GetComponent<KillSyncScript>().GetKillCount().ToString() + "   " + pl.GetComponent<PlayerScoreScript>().GetDeaths().ToString() + "   " + Mathf.RoundToInt((float)(_realtime.room.time - pl.GetComponent<PlayerScoreScript>().GetConnectTime())).ToString() + "\n";
            
        }

        _model.scoreText = t;
    }

    public void RegisterKill(int killerID)
    {
        PlayerScoreScript plScoreScript = _avatarManager.avatars[killerID].gameObject.GetComponent<PlayerScoreScript>();
        //plScoreScript.SetKills(plScoreScript.GetKills() + 1);
        plScoreScript.SetDeaths(plScoreScript.GetDeaths());
        Debug.Log(plScoreScript.GetKills());
    }
}
