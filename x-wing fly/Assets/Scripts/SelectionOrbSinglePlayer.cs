using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionOrbSinglePlayer : MonoBehaviour
{
    public int loadSceneID;

    private void OnTriggerEnter(Collider other)
    {
        PlaySP();
    }

    public void PlaySP()
    {
        GameManager.Instance.offline = true;
        gameObject.SetActive(false);
        SceneManager.LoadScene(loadSceneID);
    }
}
