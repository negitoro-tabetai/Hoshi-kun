using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumWall : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D col)
    {
       
            if (col.tag == "Pendulum")
            {
                Destroy(gameObject);
               
            }
        

    }

}
