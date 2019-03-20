using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolution : MonoBehaviour
{
    public bool On { get; set; }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (On)
        {
            Debug.Log(col.gameObject.name);
            Destroy(gameObject);
        }
    }
}