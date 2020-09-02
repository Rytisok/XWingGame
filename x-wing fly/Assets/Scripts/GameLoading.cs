using System;
using System.Collections;
using System.Collections.Generic;
using Normal.Realtime;
using TMPro;
using UnityEngine;

public enum PartsToLoad
{
    UnityRemote,
    Multiplayer,
    GraphProcessing
}
public class GameLoading : MonoBehaviour
{
    public Realtime _realtime;
    public SpaceManager spaceManager;

    public GameObject loadingIndicator;
    public TMP_Text indicatorTxt;

    public Action onLoadingDone;
    public bool loadingDone;

    private List<PartsToLoad> partsToLoad;
    void Start()
    {
        //StartLoading();
        indicatorTxt.gameObject.SetActive(false);
    }

    public void LoadWithRoom(string n)
    {
        _realtime.SetRoomName(n);
        StartLoading();
    }

    public void StartOfflineLoading(SpaceManager spaceManager,GameObject loadingIndicator, TMP_Text indicator)
    {
        this.spaceManager = spaceManager;
        this.loadingIndicator = loadingIndicator;
        indicatorTxt = indicator;
        StartLoading();
    }

    void StartLoading()
    {
        indicatorTxt.gameObject.SetActive(true);
        loadingIndicator.SetActive(true);

        StartCoroutine(VisualiseLoading());

        loadingDone = false;
        partsToLoad = new List<PartsToLoad>();

        partsToLoad.Add(PartsToLoad.UnityRemote);

        if (!GameManager.offline)
        {
            partsToLoad.Add(PartsToLoad.Multiplayer);
        }
        else
        {
            partsToLoad.Add(PartsToLoad.GraphProcessing);
        }


        if (!GameManager.offline)
        {
            _realtime.ManualConnect(Loaded);

        }
        else
        {
            spaceManager.ManualStart(Loaded);
        }


        GameManager.Instance.GetComponent<RemoteUnityManager>().StartLoading(Loaded);
    }

    void Loaded(PartsToLoad loadedPart)
    {
        Debug.Log("Loaded: "+ loadedPart);
        partsToLoad.Remove(loadedPart);
        CheckIfLoadFinished();
    }

    void CheckIfLoadFinished()
    {
        if (partsToLoad.Count == 0)
        {
            loadingDone = true;
            onLoadingDone?.Invoke();
            StopAllCoroutines();
            loadingIndicator.SetActive(false);
            Debug.Log("Loading done");
        }
    }

    IEnumerator VisualiseLoading()
    {
        int i = 0;
        while (true)
        {
            switch (i)
            {
                case 0:
                    indicatorTxt.text = "Connecting.";
                    i++;
                    break;
                case 1:
                    indicatorTxt.text = "Connecting..";
                    i++;
                    break;
                case 2:
                    indicatorTxt.text = "Connecting...";
                    i =0;
                    break;

            }
            yield return  new WaitForSeconds(0.5f);
        }
    }

}
