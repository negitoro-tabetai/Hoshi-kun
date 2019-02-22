using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Revolution : MonoBehaviour

{
   
    [SerializeField,Tooltip("弾道を表示するための点のプレファブ")]   GameObject dummyObjPref;
    
    [SerializeField,Tooltip("点をいっぱい出すので空の親オブジェクトを設定")]   Transform dummyObjParent;
    [SerializeField,Tooltip("↑と同じ")]  GameObject dummyParent;

    [SerializeField, Tooltip("ほしくん")] Transform Hoshikun;
    [SerializeField,Tooltip("初速のベクトル")]   Vector3 initalvelocity;
    
    [SerializeField,Tooltip("弾道予測の点の数")]   int dummyCount;
    
    [SerializeField,Tooltip("弾道を表示する間隔の秒数")]  float secInterval;
    
    [SerializeField,Tooltip("点が移動する速度")]  float offsetSpeed = 0.5f;
    [SerializeField, Tooltip("公転する速さ")] float speed = 300;
    [SerializeField, Tooltip("プレイヤーとどのくらい近ければ公転するかって数字")] private float distance;
     float this_distance;
     float offset;

    //弾道予測を表示するための点のリスト
    List<GameObject> dummySphereList = new List<GameObject>();

    Rigidbody rigid;
    bool enter;//弾道予測を表示してるかどうか
    bool guruguru=false;//まわすスイッチ
    bool fly_now=false;//とんでるかどうか
    

    


    void Start()
    {
        float scaleX = this.transform.localScale.x;
        float scaleY = this.transform.localScale.y;
        float scaleZ = this.transform.localScale.z;


        Off();//弾道予測はまだ表示しない
        transform.localScale = new Vector3(scaleX / 2, scaleY / 2, scaleZ / 2);//体積ちいさく
        
        this.GetComponent<BoxCollider>().isTrigger = false;//回ってる間は当たり判定オフ
        rigid = GetComponent<Rigidbody>();
        
        rigid.isKinematic = true;

        //点の開始位置
        dummyObjParent.transform.position = Hoshikun.transform.position;

        
            //弾道予測を表示するための点を生成

            for (int i = 0; i < dummyCount; i++)
            {

                var obj = (GameObject)Instantiate(dummyObjPref, dummyObjParent);

                dummySphereList.Add(obj);

            }
        

    }



    void Update()

    {

        //まわすかどうか判定
        this_distance = transform.position.x - Hoshikun.position.x;
        this_distance = System.Math.Abs(this_distance);

        if (this_distance <= distance)
        {
            if (fly_now==false)guruguru = true;//まわす
        }

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
        if (enter == true)//弾道予測がでてるときしかとばせない
        {
            if (Input.GetKeyDown(KeyCode.Return))

            {

                this.GetComponent<BoxCollider>().isTrigger = true;//判定オンに
                rigid.isKinematic = false;
                guruguru = false;
                fly_now = true;//とんでるよーという合図
                transform.localScale = new Vector3(scaleX*2, scaleY*2, scaleZ*2);
                transform.position = (dummyObjParent.transform.position);

                rigid.AddForce(initalvelocity, ForceMode.VelocityChange);
                


            }
        }
        

    }

    void OnTriggerEnter(Collider col)
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

        //Rスティック操作
        if (Input.GetKey(KeyCode.A)||Input.GetAxis("R_Horizontal")<0)
        {
            initalvelocity.x--; On();
        }
        if (Input.GetKey(KeyCode.D)||Input.GetAxis("R_Horizontal")>0)
        {
            initalvelocity.x++; On();
        }
        if (Input.GetKey(KeyCode.W)||Input.GetAxis("R_Vertical")<0)
        {
            initalvelocity.y++; On();
        }
        if (Input.GetKey(KeyCode.S)||Input.GetAxis("R_Vertical")>0)
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