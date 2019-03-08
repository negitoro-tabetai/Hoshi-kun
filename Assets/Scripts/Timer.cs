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
        text.text = 0 + ";" + 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (second > 0)
        {
            second -= Time.deltaTime *10;
        }
        text.text = minute + ":" + second.ToString("0#");
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
