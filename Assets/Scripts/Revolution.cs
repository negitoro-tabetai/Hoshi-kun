﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Revolution : MonoBehaviour

{
    
    [SerializeField,Tooltip("弾道を表示するための点(Sphere)のプレファブ")]   private GameObject dummyObjPref;
    
    
    [SerializeField, Tooltip("点をいっぱい出すので親オブジェクトを使う")]   private Transform dummyObjParent;
    [SerializeField,Tooltip("↑と同じ")]   private GameObject dummyParent;
    
    
    [SerializeField,Tooltip("初速のベクトル")]   private Vector3 initalvelocity;
    
    
    [SerializeField,Tooltip("弾道予測の点の数")]   private int dummyCount;
    
    
    [SerializeField,Tooltip("弾道を表示する間隔の秒数")]   private float secInterval;
    
    
    [SerializeField,Tooltip("点が移動する速度")]   private float offsetSpeed = 0.5f;
    
    private float offset;

    //弾道予測を表示するための点のリスト
    private List<GameObject> dummySphereList = new List<GameObject>();

    private Rigidbody rigid;
    private bool enter;
    
    void Start()
    {
        Off();
        rigid = GetComponent<Rigidbody>();

        if (!rigid)

            rigid = gameObject.AddComponent<Rigidbody>();

        rigid.isKinematic = true;

        dummyObjParent.transform.position = transform.position;

        
            //弾道予測を表示するための点を生成

            for (int i = 0; i < dummyCount; i++)
            {

                var obj = (GameObject)Instantiate(dummyObjPref, dummyObjParent);

                dummySphereList.Add(obj);

            }
        

    }



    void Update()

    {

        offset = Mathf.Repeat(Time.time * offsetSpeed, secInterval);

        //標準を操作
        Operate();

        
            //弾道予測の位置に点を移動
            for (int i = 0; i < dummyCount; i++)

            {

                var t = (i * secInterval) + offset;

                var x = t * initalvelocity.x;

                var z = t * initalvelocity.z;

                var y = (initalvelocity.y * t) - 0.5f * (-Physics.gravity.y) * Mathf.Pow(t, 2.0f);

                dummySphereList[i].transform.localPosition = new Vector3(x, y, z);

            //　↑鉛直上方投射！！！

            }



        //Enterで飛ばす


        if (enter == true)
        {
            if (Input.GetKeyDown(KeyCode.Return))

            {

                rigid.isKinematic = false;

                rigid.AddForce(initalvelocity, ForceMode.VelocityChange);



            }
        }
        

    }

    private void OnTriggerEnter(Collider col)
    {

        //ステージに当たったら消える
        if (col.tag == "Stage")
        {
            Destroy(gameObject);


            //ここで消す
           Off();


        }


    }

    void Operate()
    {

        //キャンセル
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Off();
        }

        //操作
        if (Input.GetKey(KeyCode.A))
        {
            initalvelocity.x--; On();
        }
        if (Input.GetKey(KeyCode.D))
        {
            initalvelocity.x++; On();
        }
        if (Input.GetKey(KeyCode.W))
        {
            initalvelocity.y++; On();
        }
        if (Input.GetKey(KeyCode.S))
        {
            initalvelocity.y--; On();
        }
    }

    void On()
    {
        dummyParent.SetActive(true);enter = true;
    }
    void Off()
    {
        dummyParent.SetActive(false);enter = false;
    }


}