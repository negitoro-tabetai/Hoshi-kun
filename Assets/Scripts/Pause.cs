using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Pause : MonoBehaviour
{
    private GameObject pause;
    private bool _On = false;
    private int count=0;
    [SerializeField]
    void Start()
    {
        pause = GameObject.Find("Pause");
        pause.SetActive(false);
    }

 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            pause.SetActive(true);
            _On = true;
            count += 1;
            Debug.Log(count);
        }
        if (count == 2)
        {
            count = 0;
            _On = false;
        }

        if (_On == true)
        {
            Time.timeScale = 0;
            Debug.Log("On");
          
        }
        else
        {
            Time.timeScale = 1;
            Debug.Log("Off");
            pause.SetActive(false);
        }
      
     
    }

  public void A()
    {
        SceneManager.LoadScene("Title");
    }
    public void B()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
