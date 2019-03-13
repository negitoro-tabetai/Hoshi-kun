using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataSelect : MonoBehaviour
{
    [SerializeField] Text[] _next;

    //星ごとのステージ数
    const int StagePerWorld = 5;
    void Start()
    {
        UpdateText();
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            GameManager.Instance.ReroadScene();
        }
    }
    void UpdateText()
    {
        for (int i = 0; i < _next.Length; i++)
        {
            int data = PlayerPrefs.GetInt("SaveData" + (i + 1).ToString());
            
            // データから次のステージを表示
            if (data == 35)
            {
                _next[i].text = "CLEAR";
            }
            else if (data / 10 == 0 || data % 10 == StagePerWorld)
            {
                _next[i].text = ((data / 10) + 1).ToString() + "-1";
            }
            else
            {
                _next[i].text = (data / 10).ToString() + "-" + (data % 10 + 1).ToString();
            }
        }
    }
}
