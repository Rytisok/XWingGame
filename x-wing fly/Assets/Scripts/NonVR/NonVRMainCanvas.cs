using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonVRMainCanvas : MonoBehaviour
{
    public SelectionOrb mpSelection;
    public SelectionOrbSinglePlayer spSelection;

    public SelectionOrb mpdeathmatch;
    public SelectionOrb mpteams;

    public ShipSelect xwing;
    public ShipSelect tieFighter;

    public Ship localMPShip;

    public GameObject spmpSelection;
    public GameObject mpDeahmatchTeamsSelection;
    public GameObject shipSelection;

    void Start()
    {
        if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer)
            spmpSelection.SetActive(true);
        mpDeahmatchTeamsSelection.SetActive(false);
        shipSelection.SetActive(false);


        LoadSettings();
    }

    void LoadSettings()
    {
        GameLoading loader = GameManager.Instance.GetComponent<GameLoading>();

        if (!loader.loadingDone)
        {
            loader.onLoadingDone += () => { AllowShipSelection(); };
        }
        else
        {
            AllowShipSelection();
        }

    }

    public void AllowShipSelection()
    {
        if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer)
            shipSelection.SetActive(true);

    }

    public void SelectXWing()
    {
        xwing.Select(localMPShip);

        shipSelection.SetActive(false);

    }

    public void SelectTieFighter()
    {
        tieFighter.Select(localMPShip);

        shipSelection.SetActive(false);
    }

    public void SelectMP()
    {
        mpSelection.Select();
        spmpSelection.SetActive(false);
        mpDeahmatchTeamsSelection.SetActive(true);

    }

    public void SelectSP()
    {
        spSelection.PlaySP();
        spmpSelection.SetActive(false);
        mpDeahmatchTeamsSelection.SetActive(true);

    }

    public void SelectMPDeathmatch()
    {
        mpdeathmatch.Select();

        mpDeahmatchTeamsSelection.SetActive(false);
    }

    public void SelectMPTeams()
    {
        mpteams.Select();
        mpDeahmatchTeamsSelection.SetActive(false);

    }
}
