using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SPManager : MonoBehaviour
{
    public SpaceManager spaceManager;
    public GameObject loadingIndicator;
    public TMP_Text loadingIndicatorTxt;

    void Start()
    {
        GameLoading loading = GameManager.Instance.GetComponent<GameLoading>();

        loading.StartOfflineLoading(spaceManager, loadingIndicator, loadingIndicatorTxt);

    }
}
