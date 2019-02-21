using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Revolution : MonoBehaviour

{
   
    [SerializeField,Tooltip("弾道を表示するための点のプレファブ")]   private GameObject dummyObjPref;
    
    [SerializeField,Tooltip("点をいっぱい出すので空の親オブジェクトを設定")]   private Transform dummyObjParent;
    [SerializeField,Tooltip("↑と同じ")]   private GameObject dummyParent;

    [SerializeField, Tooltip("ほしくん")] private Transform Hoshikun;
    [SerializeField,Tooltip("初速のベクトル")]   private Vector3 initalvelocity;
    
    [SerializeField,Tooltip("弾道予測の点の数")]   private int dummyCount;
    
    [SerializeField,Tooltip("弾道を表示する間隔の秒数")]   private float secInterval;
    
    [SerializeField,Tooltip("点が移動する速度")]   private float offsetSpeed = 0.5f;
    [SerializeField, Tooltip("公転する速さ")] private float speed = 300;
    private float offset;

    //弾道予測を表示するための点のリスト
    private List<GameObject> dummySphereList = new List<GameObject>();

    private Rigidbody rigid;
    private bool enter;
    private bool guruguru=true;

    


    void Start()
    {
        float scaleX = this.transform.localScale.x;
        float scaleY = this.transform.localScale.y;
        float scaleZ = this.transform.localScale.z;



        transform.localScale = new Vector3(scaleX / 2, scaleY / 2, scaleZ / 2);//体積ちいさく
        Off();
        this.GetComponent<BoxCollider>().isTrigger = false;//回ってる間は判定オフ
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
        float scaleX = this.transform.localScale.x;
        float scaleY = this.transform.localScale.y;
        float scaleZ = this.transform.localScale.z;
        //まわる！！！！！
        if (guruguru == true)
        {
            transform.RotateAround(Hoshikun.position, Hoshikun.up, speed * Time.deltaTime);
        }

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

                this.GetComponent<BoxCollider>().isTrigger = true;//判定オンに
                rigid.isKinematic = false;
                guruguru = false;
                transform.localScale = new Vector3(scaleX*2, scaleY*2, scaleZ*2);
                transform.position = (dummyObjParent.transform.position);

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