using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Timer : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] int minute;
    [SerializeField] GameObject Player;
    [SerializeField] string Scene;
    [SerializeField] float second;
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
        if (!GameManager.Instance.IsGoal)
            //秒のカウント
            if (second > 0)
            {
                second -= Time.deltaTime;
            }
        //テキストに代入
        text.text = minute + ":" + second.ToString("0#");
        //分の減少処理
        {

            if (second < 0 && minute > 0)
            {
                minute -= 1;
                second = 60;
            }
            if (minute <= 0 && second <= 0)
            {

                // SceneManager.LoadScene("StageSelect");
                FadeManager.Instance.SceneFade(Scene, 1);

            }

        }

    }
}
