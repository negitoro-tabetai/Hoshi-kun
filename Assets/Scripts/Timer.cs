using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Timer : MonoBehaviour
{
   [SerializeField] Text text;
    [SerializeField] Text GameOverText;
    [SerializeField] int minute;
    [SerializeField] GameObject PlayerH;
   private float second=59;
    private GameObject pause;
    Player player;
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
        if (second <= 0 && minute <= 0)
        {
            GameOverText.text = "GameOver";
            PlayerH.GetComponent<Player>().enabled = false;

        }
    }
}
