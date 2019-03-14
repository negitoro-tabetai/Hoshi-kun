using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Pause : MonoBehaviour
{
   
    private bool _On = false;

   

    [SerializeField] Slider _sliderSE;
    [SerializeField] Slider _sliderBGM;
    [SerializeField] GameObject panel;
    [SerializeField] GameObject pause;
    [SerializeField] GameObject option;
    void Start()
    {
      
       panel.SetActive(false);
       
        pause.SetActive(false);
       
        option.SetActive(false);

        ValueChange();
    }


    void Update()
    {
        if (option.activeSelf == false && Input.GetKeyDown(KeyCode.P))
        {
            pause.SetActive(!pause.activeSelf);
            _On = !_On;
        }


        if (_On == true)
        {
            Time.timeScale = 0;
            panel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            panel.SetActive(false);
        }


    }

    public void ReStartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Title()
    {

        SceneManager.LoadScene("Title");

    }
    public void Option()
    {
        option.SetActive(true);
        pause.SetActive(false);
    }
    public void Back()
    {
        option.SetActive(false);
        pause.SetActive(true);
    }
    public void ValueChange()
    {
        _sliderSE.value = AudioManager.Instance.SEVolume;
        _sliderBGM.value = AudioManager.Instance.BGMVolume;
    }
  
   
}
