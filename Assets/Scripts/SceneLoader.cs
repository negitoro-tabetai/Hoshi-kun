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
        SceneManager.LoadScene("1-3");
    }
    public void Stage2_1()
    {
        SceneManager.LoadScene("2-1");
    }
    public void Stage2_2()
    {
        SceneManager.LoadScene("2-2");
    }
    public void Stage3_1()
    {
        SceneManager.LoadScene("3-1");
    }
    public void Stage3_2()
    {
        SceneManager.LoadScene("3-2");
    }
    public void StageSelect2()
    {
        SceneManager.LoadScene("StageSelect2");
    }
    public void StageSelect3()
    {
        SceneManager.LoadScene("StageSelect3");
    }
    public void StageSelect()
    {
        SceneManager.LoadScene("StageSelect");
    }
    public void Update()
    {
        if (Input.GetButtonDown("UseGravity"))
        {
            if(SceneManager.GetActiveScene().name=="StageSelect")
            SceneManager.LoadScene("Title");
            else if (SceneManager.GetActiveScene().name != "Title")
            {
                SceneManager.LoadScene("StageSelect");

            }
        }
       

    }
}
