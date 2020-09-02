using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Normal.Realtime;
using TMPro;
using UnityEngine;

public class SPScoreManager : MonoBehaviour
{
    public TMP_Text text;

    private SPShip _realPlayer;

    private SPShip realPlayer
    {
        get
        {
            _realPlayer = FindObjectsOfType<SPShip>().First(x => x.gameObject.tag == "Ship");
            Debug.Log("_realPlayer FOUND: ");
            return _realPlayer;
        }
        set
        {
            _realPlayer = value;

        }
    }

    private List<SPShip> _enemybots;
    private List<SPShip> enemybots
    {
        get
        {
            _enemybots = FindObjectsOfType<SPShip>().Where(x => x.gameObject.tag  == "BotEnemy").ToList();
            Debug.Log("_enemybots FOUND: " + _enemybots.Count);
            return _enemybots;
        }
        set
        {
            _enemybots = value;

        }
    }

    public void SetScoreText()
    {
        int playerID = 0;

        string t = "";
        if (GameManager.teamGame)
        {
           /* int xwingScore = 0;
            int tieFighterScore = 0;

            List<Player> rebels = new List<Player>();
            List<Player> empire = new List<Player>();

            int count = 0;

            foreach (var plr in players)
            {
                SPPlayerScore pl = plr.GetComponent<SPPlayerScore>();
                int teamID = plr.team;

                Player p;
                p.ship = plr;
                p.playerID = count + 1;

                if (teamID == 0)
                {
                    tieFighterScore += pl.deaths;
                    rebels.Add(p);
                }
                else if (teamID == 1)
                {
                    xwingScore += pl.deaths;
                    empire.Add(p);
                }

                count++;
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
                    SPPlayerScore score = rebels[i].ship.GetComponent<SPPlayerScore>();
                    t += "Player " + rebels[i].playerID + "    " + score.kills+ "   " + score.deaths + "      ";
                }
                else
                {
                    t += "                          ";
                }

                if (empire.Count > i)
                {
                    SPPlayerScore score = empire[i].ship.GetComponent<SPPlayerScore>();

                    t += "Player " + empire[i].playerID + "    " + score.kills + "   " + score.deaths + "      ";
                }
                t += "\n";
            }*/
        }
        else
        {

            t += "Real Player " + realPlayer.GetComponent<SPPlayerScore>().kills + "   " +
                 realPlayer.GetComponent<SPPlayerScore>().deaths + "\n";

            int botKills = 0;
            int botDeaths = 0;

            foreach (var player in enemybots)
            {
                SPPlayerScore plScoreScript = player.GetComponent<SPPlayerScore>();

                botKills += plScoreScript.kills;
                botDeaths += plScoreScript.deaths;
            }

            t += "Bot Players " + botKills + "   " + botDeaths + "\n";

        }

        text.text = t;
    }

    struct Player
    {
        public SPShip ship;
        public int playerID;
    };
}
