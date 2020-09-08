using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCanvas : MonoBehaviour
{
    public GameObject deathUI;

    public Ship ship;
    public SPShip spship;

    void Start()
    {
        Hide();

        if (GameManager.Instance.offline)
        {
            spship.onDeath += Show;
            spship.onRespawn += Hide;
        }
        else
        {
            ship.onDeath += Show;
            ship.onRespawn += Hide;
        }
    }

    void Show()
    {
        deathUI.SetActive(true);
    }

    void Hide()
    {
        deathUI.SetActive(false);

    }

}
