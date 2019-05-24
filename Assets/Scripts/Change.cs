using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Change : MonoBehaviour
{
    [SerializeField] Slider _sliderSE;
    [SerializeField] Slider _sliderBGM;
    [SerializeField] GameObject A;
    [SerializeField] GameObject B;
    void Start()
    {
        A.SetActive(true);
        B.SetActive(false);
        _sliderBGM.value = AudioManager.Instance.BGMVolume;
        _sliderSE.value = AudioManager.Instance.SEVolume;
    }

    
    void Update()
    {
        
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
   public void Modoru()
    {
        A.SetActive(true);
        B.SetActive(false);

    }
    public void Tsugi()
    {
       
        A.SetActive(false);
        B.SetActive(true);
    }
   public void Starts()
    {
        SceneManager.LoadScene("1-1");
    }
   public void Selecte()
    {
        SceneManager.LoadScene("StageSelect");
    }
}
