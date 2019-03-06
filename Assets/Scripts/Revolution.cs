using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolution : MonoBehaviour
{
    [SerializeField, Tooltip("弾道を表示するための点のプレファブ")] GameObject dummyObjPref;
    [SerializeField, Tooltip("点の親オブジェクト")] GameObject dummyParent;
    [SerializeField, Tooltip("ほしくん")] GameObject Hoshikun_obj;
    [SerializeField, Tooltip("初速のベクトル")] Vector3 initalvelocity;
    //弾道予測の点の数   
    int dummyCount = 50;
    //弾道を表示する間隔の秒数
    float secInterval = 0.02f;
    //点が移動する速度
    float offsetSpeed = 0.5f;
    //公転する速さ
    float speed = 300;
    //プレイヤーとどのくらい近ければ公転するか
    float distance = 1.5f;
    [SerializeField, Tooltip("ブロックの親")] GameObject Block;
    float this_distance;
    float offset;
    float this_volume;
    float hoshi_volume;
    Rigidbody2D rigid;

    Animation animation;

    List<GameObject> dummySpheres = new List<GameObject>();//弾道予測を表示するための点のリスト
    bool enter;//弾道予測を表示してるかどうか
    bool guruguru = false;//まわすスイッチ
    bool fly_now = false;//とんでるかどうか
    bool small = true;//ちいさく

    void Start()
    {
        animation = GetComponent<Animation>();

        Off();
        rigid = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        //点の開始位置
        dummyParent.transform.position = Hoshikun_obj.transform.position;


        //まわすかどうか判定！！！！！！！！！
        this_distance = transform.position.x - Hoshikun_obj.transform.position.x;
        this_distance = System.Math.Abs(this_distance);
        //↑距離が近いか

        hoshi_volume = Hoshikun_obj.transform.localScale.x * Hoshikun_obj.transform.localScale.y * Hoshikun_obj.transform.localScale.z;
        this_volume = transform.localScale.x * transform.localScale.y * transform.localScale.z;
        //↑体積がほしくん以下か




        if (this_distance <= distance && this_volume < hoshi_volume && Hoshikun_obj.GetComponent<Revolution_Player>().Block <= 2)
        {
            if (Input.GetButtonDown("Revolution") || Input.GetKeyDown(KeyCode.R))
            {

                if (this.GetComponent<Enemy>() != null)
                {
                    GetComponent<Enemy>().enabled = false;
                }

                if (fly_now == false)
                {
                    guruguru = true; //まわす

                    if (small == true)
                    {
                        float scaleX = transform.localScale.x;
                        float scaleY = transform.localScale.y;
                        float scaleZ = transform.localScale.z;

                        //弾道予測を表示するための点を生成
                        for (int i = 0; i < dummyCount; i++)
                        {
                            var obj = (GameObject)Instantiate(dummyObjPref, dummyParent.transform);
                            dummySpheres.Add(obj);
                        }

                        ////引き寄せるスクリプトを無効に
                        //this.GetComponent<MovableBlock>().enabled = false;//
                        Destroy(GetComponent<MovableBlock>());
                        if (GetComponent<Enemy>())
                        {
                            Destroy(GetComponent<Enemy>());
                        }

                        //個数プラス
                        Hoshikun_obj.GetComponent<Revolution_Player>().AddBlock();
                        Hoshikun_obj.GetComponent<Revolution_Player>().Object[Hoshikun_obj.GetComponent<Revolution_Player>().Block] = this.gameObject;


                        rigid.isKinematic = true;
                        //ここで体積変える？？？？？

                        transform.localScale = new Vector3(scaleX / 2, scaleY / 2, scaleZ / 2);
                        small = false;
                    }
                }
            }
        }

        //まわる！！！！！
        if (guruguru == true)
        {
            Operate();//操作
            //transform.RotateAround(Hoshikun.position, Hoshikun.up, speed * Time.deltaTime);//まわす
            Block.transform.position = Hoshikun_obj.transform.position;//親オブジェクトがほしくんについていく

            

            Hoshikun_obj.GetComponent<Revolution_Player>().Animation();


            this.GetComponent<BoxCollider2D>().enabled = false;//回ってる間は当たり判定オフ

            //弾道予測の位置に点を移動
            for (int i = 0; i < dummyCount; i++)
            {
                var t = (i * secInterval) + offset;
                var x = t * initalvelocity.x;
                var z = t * initalvelocity.z;
                var y = (initalvelocity.y * t) - 0.5f * (-Physics.gravity.y) * Mathf.Pow(t, 2.0f);

                dummySpheres[i].transform.localPosition = new Vector3(x, y, z);
                //　↑鉛直上方投射の公式
            }
        }

        offset = Mathf.Repeat(Time.time * offsetSpeed, secInterval);

        //Enterで飛ばす
        if (enter == true)//弾道予測がでてるときしかとばせない
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Throw"))
            {
                Hoshikun_obj.GetComponent<Revolution_Player>().Throw_();//★★★
            }
        }
    }

    public void Throw()
    {
        rigid.isKinematic = false;//重力を考慮する

        float scaleX = transform.localScale.x;
        float scaleY = transform.localScale.y;
        float scaleZ = transform.localScale.z;

        animation.Stop();

        DestroyChildObject(dummyParent.transform);

        this.tag = "RevolutionBlock";
        this.GetComponent<BoxCollider2D>().enabled = true;//判定オンに
        this.GetComponent<BoxCollider2D>().isTrigger = true;

        guruguru = false;
        fly_now = true;//とんでる

        transform.localScale = new Vector3(scaleX * 2, scaleY * 2, scaleZ * 2);//大きさ戻す
        transform.position = (dummyParent.transform.position);
        Hoshikun_obj.GetComponent<Revolution_Player>().takeBlock();
        rigid.AddForce(initalvelocity, ForceMode2D.Impulse);//とばす
    }

    public static void DestroyChildObject(Transform parent_trans)
    {
        for (int i = 0; i < parent_trans.childCount; i++)
        {
            GameObject.Destroy(parent_trans.GetChild(i).gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
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
        if (Input.GetKey(KeyCode.LeftArrow)||Input.GetAxis("R_Horizontal")<0)
        {
            initalvelocity.x--; On();
        }
        if (Input.GetKey(KeyCode.RightArrow)||Input.GetAxis("R_Horizontal")>0)
        {
            initalvelocity.x++; On();
        }
        if (Input.GetKey(KeyCode.UpArrow)||Input.GetAxis("R_Vertical")>0)
        {
            initalvelocity.y++; On();
        }
        if (Input.GetKey(KeyCode.DownArrow)||Input.GetAxis("R_Vertical")<0)
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