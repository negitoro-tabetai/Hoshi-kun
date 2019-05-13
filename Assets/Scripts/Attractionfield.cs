using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractionfield : MonoBehaviour
{
    [SerializeField] GameObject field;
    [SerializeField] GameObject FieldCenter;
    [SerializeField] GameObject _player;
    private float speed = 10;
    private Rigidbody2D rigidbody;
    bool _Touch;
    bool On =false;
    void Start()
    {
        field.SetActive(false);
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
    }

  
    void Update()
    {
        if (!_Touch||On==true)
        {
            float step = speed * Time.deltaTime;
           // transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, step);
        }
        if(Input.GetButtonDown("Attraction")||Input.GetKeyDown(KeyCode.Y))
        {
            On = true;
            field.SetActive(false);
        }
      
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            transform.SetParent(collision.transform);
            field.SetActive(true);
            _Touch = true;
           
        }
       
    }
   
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            field.SetActive(false);
            _Touch = false;
            transform.SetParent(null);

        }
        if (collision.tag == "Player")
        {
            On = false;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            transform.SetParent(collision.transform);
            field.SetActive(true);
            _Touch = true;

        }
       if(collision.tag=="Player")
        {
            field.SetActive(false);
            transform.SetParent(collision.transform);
          //  rigidbody.isKinematic = false;
        }
    }
}
