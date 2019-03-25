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
            Player.transform.position = new Vector3(Door.transform.position.x, Player.transform.position.y, 0);
            Camera.transform.position = new Vector3(Door.transform.position.x, Camera.transform.position.y, Camera. transform.position.z);
        }
    }
}
