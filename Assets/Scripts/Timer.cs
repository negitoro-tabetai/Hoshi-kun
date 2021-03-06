﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Timer : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] Text GameOverText;
    [SerializeField] int minute;
    [SerializeField] GameObject Player;
    [SerializeField] string Scene;

    private float second=60f;
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
            second -= Time.deltaTime;
        }

        //分の減少処理
        if (second < 0 && minute > 0)
        {
            minute -= 1;
            second = 60;
        }

        //テキストに代入する
        text.text = minute + ":" + second.ToString("0#");

        //残り時間が0になったら
        if ( minute <= 0&& second <= 0)
        {
          GameOverText.text = "GameOver";
          Player.GetComponent<Player>().enabled = false;
          FadeManager.Instance.SceneFade(Scene, 1);
        }     
    }
}
