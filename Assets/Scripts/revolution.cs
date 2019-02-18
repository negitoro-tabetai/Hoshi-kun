using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Revolution : MonoBehaviour

{
    //弾道を表示するための点のプレハブ
    [SerializeField]   private GameObject dummyObjPref;
    
    //弾道を表示する点の親オブジェクト
    [SerializeField]   private Transform dummyObjParent;
    
    //初速のベクトル
    [SerializeField]   private Vector3 initalvelocity;
    
    //弾道予測の点の数
    [SerializeField]   private int dummyCount;
    
    //弾道を表示する間隔の秒数
    [SerializeField]   private float secInterval;
    
    //点が移動する速度
    [SerializeField]   private float offsetSpeed = 0.5f;
    
    private float offset;
    
    private List<GameObject> dummySphereList = new List<GameObject>();

    private Rigidbody rigid;

    
    void Start()
    {
        
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



        //弾道予測の位置に点を移動

        for (int i = 0; i < dummyCount; i++)

        {

            var t = (i * secInterval) + offset;

            var x = t * initalvelocity.x;

            var z = t * initalvelocity.z;

            var y = (initalvelocity.y * t) - 0.5f * (-Physics.gravity.y) * Mathf.Pow(t, 2.0f);

            dummySphereList[i].transform.localPosition = new Vector3(x, y, z);

        }



        //スペースキーで球の弾道を確認

        if (Input.GetKeyDown(KeyCode.Space))

        {

            rigid.isKinematic = false;

            rigid.AddForce(initalvelocity, ForceMode.VelocityChange);

        }

    }

    void Operate()
    {

    }


}