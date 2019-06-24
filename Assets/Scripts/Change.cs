using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Change : MonoBehaviour
{
    [SerializeField] Slider _sliderSE;
    [SerializeField] Slider _sliderBGM;
    [SerializeField] GameObject TitleObject;
    [SerializeField] GameObject OptionObject;

    void Start()
    {
        TitleObject.SetActive(true);
        OptionObject.SetActive(false);

        //保存されているSE,BGMの値参照、それをスライダーのvalueに入れる
        _sliderBGM.value = AudioManager.Instance.BGMVolume;
        _sliderSE.value = AudioManager.Instance.SEVolume;
    }
    //タイトルシーンでのオプション画面中の、SE変更用
    public void ChangeVolumeSE()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            _sliderSE.value += Mathf.Sign(Input.GetAxisRaw("Horizontal")) * 0.01f;
            Mathf.Clamp01(_sliderSE.value);
            AudioManager.Instance.ChangeVolumeSE(_sliderSE.value);
        }
    }

    //タイトルシーンでのオプション画面中の、BGM変更用
    public void ChangeVolumeBGM()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            _sliderBGM.value += Mathf.Sign(Input.GetAxisRaw("Horizontal")) * 0.01f;
            Mathf.Clamp01(_sliderBGM.value);
            AudioManager.Instance.ChangeVolumeBGM(_sliderBGM.value);
        }
    }

    //オプション画面からタイトルに戻る
   public void Back()
    {
        TitleObject.SetActive(true);
        OptionObject.SetActive(false);
    }

    //タイトルからオプション画面に進む
    public void Next()
    {      
        TitleObject.SetActive(false);
        OptionObject.SetActive(true);
    }

    //ステージ1-1シーンにFadeManagerで遷移する
   public void Starts()
    {
        FadeManager.Instance.SceneFade("1-1", 0.5f);
    }

    //ステージTimeTrialシーンにFadeManagerで遷移する
    public void Selecte()
    {
        FadeManager.Instance.SceneFade("TimeTrial", 0.5f);
    }
}
