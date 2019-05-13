using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GrantBlock : MonoBehaviour
{
    [SerializeField] GameObject[] _blockPrefab = new GameObject[3];
    bool _isOn = false;

    [SerializeField] Image Star;

    public bool IsOn
    {
        get
        {
            return _isOn;
        }
    }


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
            _isOn = !_isOn;
            Switch(_isOn);
        }
    }
}
