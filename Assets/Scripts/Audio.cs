using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  Audio : MonoBehaviour
{
   
    void Start()
    {
       
            AudioManager.Instance.PlayBGM("spring");
        
    }
     void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.Instance.PlaySE("Jump");
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            AudioManager.Instance.PlaySE("Pause");
        }
    }
}
   


   
   

