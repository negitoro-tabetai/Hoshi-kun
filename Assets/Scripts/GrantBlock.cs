using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GrantBlock : MonoBehaviour
{

    public bool IsOn { get; set; }

    /// <summary>
    /// スイッチonとoffで色を変える
    /// </summary>
    /// <param name="isTouch">スイッチがonかoffか</param>
    void Switch(bool isTouch)
    {
        if (isTouch)
        {
            //GetComponent<Renderer>().material.color = Color.blue;
          //  Star.sprite = On;
        }
        else
        {
            //GetComponent<Renderer>().material.color = Color.black;
       //     Star.sprite = Off;
        }
    }


    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            IsOn = !IsOn;
            Switch(IsOn);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if((collision.gameObject.tag == "Player" || 
            collision.gameObject.tag == "RevolutionBlock"))
        {
            IsOn = !IsOn;
            Switch(IsOn);
        }
    }
}
