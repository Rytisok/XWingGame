using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionOrbSinglePlayer : MonoBehaviour
{
    public int loadSceneID;

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
        SceneManager.LoadScene(loadSceneID);
    }
}
