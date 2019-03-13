using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Pause : MonoBehaviour
{
    private GameObject pause;
    private GameObject option;
    private GameObject panel;
    private bool _On = false;

   

    [SerializeField] Slider _sliderSE;
    [SerializeField] Slider _sliderBGM;
    void Start()
    {
        panel = GameObject.Find("Panel");
        panel.SetActive(false);
        pause = GameObject.Find("Pause");
        pause.SetActive(false);
        option = GameObject.Find("Option");
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
    public void ChangeVolumeSE()
    {

        if (Input.GetKey(KeyCode.RightArrow) && _sliderSE.value < 1)
        {
            _sliderSE.value += 0.01f;
            AudioManager.Instance.ChangeVolumeSE(_sliderSE.value);

        }
        if (Input.GetKey(KeyCode.LeftArrow) && _sliderSE.value > 0)
        {
            _sliderSE.value -= 0.01f;
            AudioManager.Instance.ChangeVolumeSE(_sliderSE.value);
        }

    }
    public void ChangeVolumeBGM()
    {

        if (Input.GetKey(KeyCode.RightArrow) && _sliderBGM.value < 1)
        {
            _sliderBGM.value += 0.01f;
            AudioManager.Instance.ChangeVolumeBGM(_sliderBGM.value);
        }
        if (Input.GetKey(KeyCode.LeftArrow) && _sliderBGM.value > 0)
        {
            _sliderBGM.value -= 0.01f;
            AudioManager.Instance.ChangeVolumeBGM(_sliderBGM.value);
        }

    }
   
}
