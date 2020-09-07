using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonVRMainCanvas : MonoBehaviour
{
    public SelectionOrb mpSelection;
    public SelectionOrbSinglePlayer spSelection;

    public SelectionOrb mpdeathmatch;
    public SelectionOrb mpteams;

    public GameObject spmpSelection;
    public GameObject mpDeahmatchTeamsSelection;

    void Start()
    {
        spmpSelection.SetActive(true);
        mpDeahmatchTeamsSelection.SetActive(false);
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
