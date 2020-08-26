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
   // public SpaceManager spaceManager;

    public GameObject LoadingIndicator;
    public TMP_Text indicatorTxt;

    public Action onLoadingDone;
    public bool loadingDone;

    private List<PartsToLoad> partsToLoad;

    void Start()
    {
        StartLoading();
    }

    void StartLoading()
    {
        LoadingIndicator.SetActive(true);

        StartCoroutine(VisualiseLoading());

        loadingDone = false;
        partsToLoad = new List<PartsToLoad>();

        partsToLoad.Add(PartsToLoad.UnityRemote);
        partsToLoad.Add(PartsToLoad.Multiplayer);

        RemoteUnityManager.Instance.StartLoading(Loaded);

        //   partsToLoad.Add(PartsToLoad.GraphProcessing);
       _realtime.ManualConnect(Loaded);
      //   spaceManager.ManualStart(Loaded);
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
            LoadingIndicator.SetActive(false);
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
