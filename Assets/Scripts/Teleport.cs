using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] GameObject Door;
    [SerializeField] GameObject Camera;
    void Start()
    {
        
    }

   
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag== "Player")
        {
            Player.transform.position = Door.transform.position;
           Camera.transform.position = Door.transform.position;
        }
    }
}
