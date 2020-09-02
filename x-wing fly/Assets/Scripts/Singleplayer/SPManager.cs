using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SPManager : MonoBehaviour
{
    public SpaceManager spaceManager;
    public GameObject loadingIndicator;
    public TMP_Text loadingIndicatorTxt;

    public ShipSelect team1Ship;
    public ShipSelect team2Ship;

    void Start()
    {
        GameLoading loading = GameManager.Instance.GetComponent<GameLoading>();

        team1Ship.loader = loading;
        team2Ship.loader = loading;

        team1Ship.Initialize();
        team2Ship.Initialize();

        loading.StartOfflineLoading(spaceManager, loadingIndicator, loadingIndicatorTxt);

    }
}
