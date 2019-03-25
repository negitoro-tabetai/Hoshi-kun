using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolution : MonoBehaviour
{
    [SerializeField] GameObject _revolutionBreakEffect;
    public bool On { get; set; }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (On)
        {
            Instantiate(_revolutionBreakEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}