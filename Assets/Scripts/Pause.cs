using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Pause : MonoBehaviour
{

    [SerializeField] Slider _sliderSE;
    [SerializeField] Slider _sliderBGM;
    [SerializeField] GameObject panel;
    [SerializeField] GameObject pause;
    [SerializeField] GameObject option;
    bool _IsPause=false;
    void Start()
    {

        panel.SetActive(false);

        pause.SetActive(false);

        option.SetActive(false);

        //AudioManagerと同じ値にしておく
        _sliderSE.value = AudioManager.Instance.SEVolume;
        _sliderBGM.value = AudioManager.Instance.BGMVolume;
    }


    void Update()
    {
        if (Input.GetButtonDown("Pause") && !option.activeSelf && Input.GetButtonDown("Pause") && !FadeManager.Instance.IsFading)
        {
            pause.SetActive(!pause.activeSelf);
            GameManager.Instance.IsPausing = !GameManager.Instance.IsPausing;

            //★
            if (!_IsPause)
            {
                AudioManager.Instance.PlaySE("Pause"); _IsPause = true;
            }
            else if (_IsPause)
            {
                AudioManager.Instance.PlaySE("PauseCancel"); _IsPause = false;
            }

        }


        if (GameManager.Instance.IsPausing)
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
        GameManager.Instance.IsPausing = false;
        GameManager.Instance.ResetPoint();
        FadeManager.Instance.SceneFade(SceneManager.GetActiveScene().name, 0.2f);
    }
    public void Title()
    {
        GameManager.Instance.IsPausing = false;
        GameManager.Instance.ResetPoint();
        FadeManager.Instance.SceneFade("Title", 0.5f);

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
    public void ChangeVolumeSE()
    {

        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            _sliderSE.value += Mathf.Sign(Input.GetAxisRaw("Horizontal")) * 0.01f;
            Mathf.Clamp01(_sliderSE.value);
            AudioManager.Instance.ChangeVolumeSE(_sliderSE.value);
        }

    }
    public void ChangeVolumeBGM()
    {

        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            _sliderBGM.value += Mathf.Sign(Input.GetAxisRaw("Horizontal")) * 0.01f;
            Mathf.Clamp01(_sliderBGM.value);
            AudioManager.Instance.ChangeVolumeBGM(_sliderBGM.value);
        }
    }
}