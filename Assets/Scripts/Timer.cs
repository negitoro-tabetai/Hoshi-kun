using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Timer : MonoBehaviour
{
   [SerializeField] Text text;
    [SerializeField] int minute;
   private float second=59;
    private GameObject pause;
    void Start()
    {
        pause = GameObject.Find("Pause");
        //初期値
        text.text = 0 + ";" + 0;
    }

   
    void Update()
    {
        //秒のカウント
        if (second > 0)
        {
            second -= Time.deltaTime ;
        }
        //テキストに代入
        text.text = minute + ":" + second.ToString("0#");
        //分の減少処理
        if (second < 0&&minute>0)
        {
            minute -= 1;
            second = 60;
        }
        if (second < 0 && minute < 0)
        {
            
        }
    }
}
