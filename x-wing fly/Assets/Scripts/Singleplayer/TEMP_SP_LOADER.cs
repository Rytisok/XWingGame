using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TEMP_SP_LOADER : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.offline = true;
        SceneManager.LoadScene(1);
    }
}
