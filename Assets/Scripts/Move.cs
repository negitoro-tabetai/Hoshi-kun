using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    
    [SerializeField]
    GameObject Startobj;
    [SerializeField]
    GameObject Endobj;
    private float elapsedTime;
    private Vector3 StartPos;
    private Vector3 EndPos;
    public float time;
    private bool Moves=false;
    private bool HitStart=false;
    private bool HitEnd = false;
    void Start()
    {
        StartPos = Startobj.gameObject.transform.position;
        EndPos = Endobj.gameObject.transform.position;
        this.transform.position = StartPos; 
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            Moves = true;
            HitStart = false;
            transform.position += ((EndPos - StartPos) / time) * Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.V))
        {
            Moves = false;           
        }
        if (elapsedTime >= 5)
        {
            HitEnd = false;
        }
        
    }

     void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Start")
        {
            Debug.Log("Hit");
            HitStart = true;
        }
        if (other.gameObject.tag == "End")
        {
            Debug.Log("Hit2");
            HitEnd = true;
        }     
    }
     void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Start")
        {
           // HitStart = true;
        }
            if (other.gameObject.tag == "End")
        {
            elapsedTime += Time.deltaTime;
            Debug.Log(elapsedTime);
        }
    }
    void FixedUpdate()
    {
        if (Moves)
        {
             transform.position += ((EndPos - StartPos) / time) * Time.deltaTime;                  
        }
        else 
        {
            transform.position += ((StartPos - EndPos) / time) * Time.deltaTime;
        }
        if (HitEnd)
        {
            transform.position = EndPos;
        }
        if (HitStart)
        {
            transform.position = StartPos;
        }
    }
    void Ray()
    {
        int distance = 10;
        Ray ray = new Ray(transform.position, new Vector3(0,-1,0));
        Debug.DrawLine(ray.origin, ray.direction * distance, Color.red);
    }
}
