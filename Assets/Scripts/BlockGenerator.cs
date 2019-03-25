using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlockGenerator : MonoBehaviour
{
   
    [SerializeField, Tooltip("ブロック")] GameObject[] block;
    [SerializeField, Tooltip("生成するブロックのプレファブ")] GameObject blockprefab;
 
    public Swich swich;
    bool stop=true;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {


        block = GameObject.FindGameObjectsWithTag("RevolutionBlock");//公転ブロックのタグを取得
        if (block.Length == 0 && swich.Swich_ == false)//リスト内のオブジェクトがなくなってもスイッチがオフだったら
        {
            
            if (stop == true)
            {
                Debug.Log("ほい");
                Instantiate(blockprefab, this.transform.position, Quaternion.identity);//ブロック生成
                stop = false;
            }
            stop = true;
        }
    }
}
