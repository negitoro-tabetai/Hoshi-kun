using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    //プレイヤー
    [SerializeField]
    GameObject player;
    //移動スピード
    [SerializeField]
    int speed;
    [SerializeField, Tooltip("接地判定するレイヤー")] LayerMask _groundLayer;
    [SerializeField, Tooltip("Rayの長さ")] float _rayLength;
    [SerializeField, Tooltip("Rayの飛ばす範囲")] float _width;

    //private Rigidbody rb;
    private GameObject Cube;
    [SerializeField]
     Rigidbody rb;
   
    void Start()
    {

        rb = GetComponent<Rigidbody>();
      
        rb.isKinematic = true;
       
    }
    void Update()
    {
        //ブロックの位置
        var pos = this.gameObject.transform.position;



        this.gameObject.transform.position = pos;


    }

    bool IsGrounded()
    {
        Ray rayRight = new Ray(transform.position + new Vector3(_width, 0, 0), Vector3.down);
        Ray rayLeft = new Ray(transform.position + new Vector3(-_width, 0, 0), Vector3.down);

        bool isGrounded = Physics.Raycast(rayLeft, _rayLength, _groundLayer) || Physics.Raycast(rayRight, _rayLength, _groundLayer);

        return isGrounded;
    }
  
   


    void OnTriggerStay(Collider other)



    {
        //レイ飛ばして範囲内で動くか
      
     
        if (other.gameObject.tag == "Field" && IsGrounded())
        {




            this.gameObject.GetComponent<Rigidbody>().velocity = (player.transform.position - this.gameObject.transform.position).normalized * speed;

            rb.isKinematic = false;


        }
     
       

    }
    //プレイヤーに当たったとき
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
          rb.isKinematic = true;


        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Field" && IsGrounded())
        {
            rb.isKinematic = true;
        
        }
        
    }

}
