﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrantBlock : MonoBehaviour
{
    [SerializeField] GameObject[] _blockPrefab = new GameObject[3];
    bool _isOn = false;

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
            GetComponent<Renderer>().material.color = Color.blue;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.black;
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if((collision.gameObject.tag == "Player" || 
            collision.gameObject.tag == "RevolutionBlock"))
        {
            if (_isOn)
            {
                _isOn = false;
            }
            else
            {
                _isOn = true;
            }
            Switch(_isOn);
        }
    }
}