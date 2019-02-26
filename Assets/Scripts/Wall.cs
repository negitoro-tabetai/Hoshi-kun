using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    bool on;
    public Swich swich;
    bool wall = true;
    
    // Start is called before the first frame update
    void Start()
    {
      
    }
   
    // Update is called once per frame
    void Update()
    {
        if (wall == true)
        {
            on = swich.Swich_;
            if (on == true)
            {
                Debug.Log("オン");
                StartCoroutine("Up");
                wall = false;
            }
        }
    }
    IEnumerator Up()
    {
        Vector3 position = transform.position;
        while (transform.localScale.y>=0)
        {
            
            position.y += 0.1f;
            transform.localScale -= new Vector3(0, 0.1f, 0);
            transform.position = position;
            yield return new WaitForSeconds(0.01f);
        }
        GetComponent<Wall>().enabled = false;
    }
}
