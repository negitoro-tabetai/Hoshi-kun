using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
   public void Stage1()
    {
        SceneManager.LoadScene("1-1");
    }
    public void Stage2()
    {
        SceneManager.LoadScene("1-2");
    }
    public void Stage3()
    {
        SceneManager.LoadScene("Haruka-Stage1-3(Start)");
    }
}
