using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swich : MonoBehaviour
{

    [SerializeField] bool swich = false;
    
    public bool Swich_
    {
        get
        {
            return swich;
        }
        set
        {

        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.tag == "RevolutionBlock")
        {
            swich = true;
            Debug.Log("オン");
            Destroy(gameObject);
        }
    }
}
