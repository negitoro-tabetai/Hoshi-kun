using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Pause : MonoBehaviour
{

    [SerializeField, Tooltip("SE用スライダー")] Slider _sliderSE;
    [SerializeField, Tooltip("BGM用スライダー")] Slider _sliderBGM;
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

            //ポーズの専用音再生(SE)★
            if (!_IsPause)
            {
                AudioManager.Instance.PlaySE("Pause"); _IsPause = true;
            }
            else if (_IsPause)
            {
                AudioManager.Instance.PlaySE("PauseCancel"); _IsPause = false;
            }
        }

        //ゲームシーンでのポーズ用
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

    //ゲームシーン、リスタート用(ステージの初めから)
    public void ReStartScene()
    {
        GameManager.Instance.IsPausing = false;
        GameManager.Instance.ResetPoint();
        FadeManager.Instance.SceneFade(SceneManager.GetActiveScene().name, 0.2f);
    }

    //ゲームシーン、タイトル遷移用
    public void Title()
    {
        GameManager.Instance.IsPausing = false;
        GameManager.Instance.ResetPoint();
        FadeManager.Instance.SceneFade("Title", 0.5f);
    }

    //オプション画面の表示,非表示用
    public void Option()
    {
        option.SetActive(true);
        pause.SetActive(false);
    }

    //オプション画面表示中にゲーム画面に戻る
    public void Back()
    {
        option.SetActive(false);
        pause.SetActive(true);    
    }

    //オプション画面表示中のSE変更用
    public void ChangeVolumeSE()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            _sliderSE.value += Mathf.Sign(Input.GetAxisRaw("Horizontal")) * 0.01f;
            Mathf.Clamp01(_sliderSE.value);
            AudioManager.Instance.ChangeVolumeSE(_sliderSE.value);
        }
    }

    //オプション画面表示中のBGM変更用
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