using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    [SerializeField] private GameObject effect;
    void OnTriggerEnter2D(Collider2D col)
    {
        //パーティクル出す
        Instantiate(effect, transform.position, transform.rotation);
    }
}
