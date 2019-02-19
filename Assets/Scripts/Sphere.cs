using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    [SerializeField] private GameObject effect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider col)
    {

        
        if (col.tag == "Stage")
        {
            

            //パーティクル出す
            Instantiate(effect, transform.position, transform.rotation);
            


        }


    }
}
