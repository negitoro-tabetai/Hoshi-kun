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
    }

    
    void Update()
    {
        
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
        SceneManager.LoadScene("DataSelect");
    }
}
