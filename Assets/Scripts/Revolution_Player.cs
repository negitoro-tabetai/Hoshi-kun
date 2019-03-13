using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolution_Player : MonoBehaviour
{
    [SerializeField, Tooltip("弾道を表示するための点のプレファブ")] GameObject dummyObjPref;
    [SerializeField, Tooltip("点の親オブジェクト")] GameObject dummyParent;
    [SerializeField, Tooltip("初速のベクトル")] public Vector3 initalvelocity;
    //弾道予測の点の数   
    int dummyCount = 50;
    //弾道を表示する間隔の秒数
    float secInterval = 0.02f;
    //点が移動する速度
    float offsetSpeed = 0.5f;
    //公転する速さ
    float speed = 300;
    
    float offset;
  

    List<GameObject> dummySpheres = new List<GameObject>();//弾道予測を表示するための点のリスト
   public List<GameObject> Object = new List<GameObject>(); //公転可能なオブジェクト
   public List<GameObject> RevolutionObject = new List<GameObject>(); //公転してるオブジェクト

    bool enter;//弾道予測を表示してるかどうか
    bool guruguru = false;//まわすスイッチ
    bool throw_ = false;//とんでるかどうか
    bool small = true;//ちいさくするスイッチ

    
    // Start is called before the first frame update
    void Start()
    {
        Off();
    }

    void Update()
    {
        //点の開始位置をほしくんの現在の位置に
        dummyParent.transform.position = this.transform.position;

        //回せるか判定
        if (Object.Count > 0)//公転可能なオブジェクトが1つでもあれば
        {
            
            if (Input.GetButtonDown("Revolution") || Input.GetKeyDown(KeyCode.R))     //公転ボタン押す
            {
                small = true;
                Debug.Log("公転");
                //公転可能リストから公転中リストへ
                RevolutionObject.Add(Object[0]); 
                Object.Remove(Object[0]);

  
                
                if (throw_ == false)
                {
                    Debug.Log("公転その２");
                    guruguru = true;

                    if (small == true)
                    {
                        //位置(Y)調整
                        RevolutionObject[RevolutionObject.Count - 1].transform.position = new Vector3(RevolutionObject[RevolutionObject.Count - 1].transform.position.x, this.transform.position.y,
                            RevolutionObject[RevolutionObject.Count - 1].transform.position.z);


                        float scaleX = RevolutionObject[RevolutionObject.Count - 1].transform.localScale.x;
                        float scaleY = RevolutionObject[RevolutionObject.Count - 1].transform.localScale.y;
                        float scaleZ = RevolutionObject[RevolutionObject.Count - 1].transform.localScale.z;

                        //弾道予測を表示するための点を生成
                        for (int i = 0; i < dummyCount; i++)
                        {
                            var obj = (GameObject)Instantiate(dummyObjPref, dummyParent.transform);
                            dummySpheres.Add(obj);
                        }

                        //引き寄せるスクリプトを無効に
                        Destroy(RevolutionObject[RevolutionObject.Count-1]. GetComponent<MovableBlock>());
                        if (RevolutionObject[RevolutionObject.Count - 1].GetComponent<Enemy>())
                        {
                            Destroy(RevolutionObject[RevolutionObject.Count - 1].GetComponent<Enemy>());
                        }

                        RevolutionObject[RevolutionObject.Count-1].GetComponent<Rigidbody2D>().isKinematic = true;

                        RevolutionObject[RevolutionObject.Count - 1].transform.parent.gameObject.transform.parent = this.transform;//プレイヤーの子に

                        //体積変更
                        RevolutionObject[RevolutionObject.Count - 1].transform.localScale = new Vector3(scaleX / 2, scaleY / 2, scaleZ / 2);
                        Debug.Log("小さくした");
                        small = false;
                    }
                }
            }
        }

        //まわる！！！！！
        if (guruguru == true)
        {

            if (RevolutionObject.Count > 0)
            {
                Operate();//操作


                RevolutionObject[RevolutionObject.Count - 1].transform.RotateAround(transform.position, transform.up, 300 * Time.deltaTime);//まわす

                //RevolutionObject[RevolutionObject.Count - 1].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;


                if (RevolutionObject.Count == 2)
                {
                    RevolutionObject[RevolutionObject.Count - 2].transform.RotateAround(transform.position, transform.up, 300 * Time.deltaTime);
                    Test(1, 2);
                }
                else
                if (RevolutionObject.Count == 3)
                {
                    RevolutionObject[RevolutionObject.Count - 2].transform.RotateAround(transform.position, transform.up, 300 * Time.deltaTime);
                    RevolutionObject[RevolutionObject.Count - 3].transform.RotateAround(transform.position, transform.up, 300 * Time.deltaTime);
                    Test(2, 3); Test(1, 3);
                }

                RevolutionObject[RevolutionObject.Count - 1].GetComponent<BoxCollider2D>().enabled = false;//回ってる間は当たり判定オフ

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
        }

        offset = Mathf.Repeat(Time.time * offsetSpeed, secInterval);

        //Enterで飛ばす
        if (enter == true)//弾道予測がでてるときしかとばせない
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Throw"))
            {
               Throw();//★★★
            }
        }
    }

    public void Throw()
    {
        RevolutionObject[0].GetComponent<Rigidbody2D>().isKinematic = false;//重力

        float scaleX = RevolutionObject[0].transform.localScale.x;
        float scaleY = RevolutionObject[0].transform.localScale.y;
        float scaleZ = RevolutionObject[0].transform.localScale.z;

        //DestroyChildObject(dummyParent.transform);


        RevolutionObject[0].tag = "RevolutionBlock";
        RevolutionObject[0].GetComponent<BoxCollider2D>().enabled = true;//判定オンに
        RevolutionObject[0].GetComponent<BoxCollider2D>().isTrigger = true;


        throw_ = true;//とんでる

        RevolutionObject[0].transform.localScale = new Vector3(scaleX * 2, scaleY * 2, scaleZ * 2);//大きさ戻す
        RevolutionObject[0].transform.position = (dummyParent.transform.position);
        RevolutionObject[0].transform.rotation = new Quaternion(0, 0, 0, 0);//Rotationを戻す

        RevolutionObject[0].GetComponent<Rigidbody2D>().AddForce(initalvelocity, ForceMode2D.Impulse);//とばす
        Debug.Log("投げた");
        RevolutionObject.Remove(RevolutionObject[0]);
        throw_ = false;

    }

    public static void DestroyChildObject(Transform parent_trans)
    {
        for (int i = 0; i < parent_trans.childCount; i++)
        {
            GameObject.Destroy(parent_trans.GetChild(i).gameObject);
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
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("R_Horizontal") < 0)
        {
            initalvelocity.x--; On();
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("R_Horizontal") > 0)
        {
            initalvelocity.x++; On();
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetAxis("R_Vertical") > 0)
        {
            initalvelocity.y++; On();
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetAxis("R_Vertical") < 0)
        {
            initalvelocity.y--; On();
        }
    }

   public void On()
    {
        dummyParent.SetActive(true); enter = true;
    }
   public void Off()
    {
        dummyParent.SetActive(false); enter = false;
    }

    void Test(int farst,int second)
    {
       

        float x = RevolutionObject[RevolutionObject.Count - farst].transform.position.x -
                  RevolutionObject[RevolutionObject.Count - second].transform.position.x;
        x = System.Math.Abs(x);

        float z=  RevolutionObject[RevolutionObject.Count - farst].transform.position.z -
                   RevolutionObject[RevolutionObject.Count - second].transform.position.z;
        z = System.Math.Abs(z);


        if (x < 1 && z < 0.3)
        {
            Debug.Log("近いので調整");
            RevolutionObject[RevolutionObject.Count - farst].transform.position = new Vector3(RevolutionObject[RevolutionObject.Count - farst].transform.position.x + 0.2f,
              RevolutionObject[RevolutionObject.Count - farst].transform.position.y, RevolutionObject[RevolutionObject.Count - farst].transform.position.z + 0.2f);

        }
    }


}
