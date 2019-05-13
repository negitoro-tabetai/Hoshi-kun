using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpStand : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] float jump;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _player.GetComponent<Rigidbody2D>().AddForce((transform.up+transform.right) * jump * Time.deltaTime, ForceMode2D.Impulse);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _player.GetComponent<Rigidbody2D>().AddForce((transform.up + transform.right) * jump * Time.deltaTime, ForceMode2D.Impulse);
        }
    }
}
