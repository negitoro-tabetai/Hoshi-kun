using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            SceneManager.LoadScene("StageSelect");
        }
    }

    public void ReroadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
