using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrantBlock : MonoBehaviour
{
    [SerializeField] GameObject[] _blockPrefab = new GameObject[3];
    bool _isTouching = false;
    // Start is called before the first frame update
    void Start()
    { 

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// スイッチonとoffで色を変える
    /// </summary>
    /// <param name="isTouch">スイッチがonかoffか</param>
    void Switch(bool isTouch)
    {
        if (isTouch)
        {
            GetComponent<Renderer>().material.color = Color.blue;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.black;
        }
    }





    void OnCollisionEnter(Collision collision)
    {
        if((collision.gameObject.tag == "Player" || collision.gameObject.tag == "RevolutionBlock"))
        {
            if (_isTouching)
            {
                _isTouching = false;
                Debug.Log(_isTouching);
            }
            else
            {
                _isTouching = true;
                Debug.Log(_isTouching);
            }
            Switch(_isTouching);
        }
    }
}
