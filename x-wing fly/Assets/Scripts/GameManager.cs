using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region GameManager Instance

    private static GameManager instance = null;

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        Application.targetFrameRate = 60;

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;//Avoid doing anything else
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    private bool _offline;

    public bool offline
    {

        get { return _offline; }
        set
        {
            _offline = value;
            GetComponent<GameLoading>().onLoadingDone = null;
        }
    
    }
    public static bool teamGame = false;

}
