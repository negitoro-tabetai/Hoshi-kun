using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySource : MonoBehaviour
{
    [SerializeField, Tooltip("引力の範囲の表示")] GameObject _field;
    public bool IsUsingGravity { get; set; }


    void Start()
    {
        
    }

    void Update()
    {
        // 範囲表示
        if (IsUsingGravity)
        {
            _field.SetActive(true);
        }
        else
        {
            _field.SetActive(false);
        }
    }
}
