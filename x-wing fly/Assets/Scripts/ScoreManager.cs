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
        GameLoading loader = RemoteUnityManager.Instance.GetComponent<GameLoading>();

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
        if (_realtime.GetCurrentRoomName() == "t")
        {
            int xwingScore = 0;
            int tieFighterScore = 0;

            List<Player> rebels = new List<Player>();
            List<Player> empire = new List<Player>();

            foreach (var plr in _avatarManager.avatars)
            {
                PlayerScoreScript pl = _avatarManager.avatars[plr.Key].GetComponent<PlayerScoreScript>();
                TeamSync team = _avatarManager.avatars[plr.Key].GetComponent<TeamSync>();

                Player p;
                p.obj = pl.gameObject;
                p.playerID = plr.Key + 1;

                if (team.GetTeam() == 0)
                {
                    tieFighterScore += pl.GetDeaths();
                    rebels.Add(p);
                }
                else if(team.GetTeam() == 1)
                {
                    xwingScore += pl.GetDeaths();
                    empire.Add(p);
                }
            }

            t += "Rebels    " + xwingScore.ToString() + "       " + tieFighterScore + "    Empire" + "\n";
            t += "Name  K   D           Name    K   D  \n";

            int biggerListCount;
            if (rebels.Count > empire.Count)
            {
                biggerListCount = rebels.Count;
            }
            else
            {
                biggerListCount = empire.Count;
            }

            for (int i = 0; i < biggerListCount; i++)
            {
                if (rebels.Count > i)
                {
                    t += "Player " + rebels[i].playerID + "    " + rebels[i].obj.GetComponent<KillSyncScript>().GetKillCount().ToString() + "   " + rebels[i].obj.GetComponent<PlayerScoreScript>().GetDeaths().ToString() + "      ";
                }
                else
                {
                    t += "                          ";
                }

                if (empire.Count > i)
                {
                    t += "Player " + empire[i].playerID + "    " + empire[i].obj.GetComponent<KillSyncScript>().GetKillCount().ToString() + "   " + empire[i].obj.GetComponent<PlayerScoreScript>().GetDeaths().ToString();
                }
                t += "\n";
            }
        }
        else
        {
            foreach (var player in _avatarManager.avatars)
            {
                playerID = player.Key + 1;
                PlayerScoreScript plScoreScript = _avatarManager.avatars[player.Key].gameObject.GetComponent<PlayerScoreScript>();
                int tmp = plScoreScript.GetKills();

                t += "Player " + playerID + "    " + _avatarManager.avatars[player.Key].gameObject.GetComponent<KillSyncScript>().GetKillCount() + "   " + plScoreScript.GetDeaths().ToString() + Mathf.RoundToInt((float)(_realtime.room.time - plScoreScript.GetConnectTime())).ToString() + "\n";
            }
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

    struct Player
    {
        public GameObject obj;
        public int playerID;
    };
}
