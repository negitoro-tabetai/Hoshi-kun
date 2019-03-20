using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlockGenerator : MonoBehaviour
{
   
    [SerializeField, Tooltip("ブロック")] GameObject block;
    [SerializeField, Tooltip("生成するブロックのプレファブ")] GameObject blockprefab;
    bool on;
    public Swich swich;
    bool stop=true;
    // Start is called before the first frame update
    void Start()
    {
        //Instantiate(blockprefab, this.transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.P))
        // {
        //     SceneManager.LoadScene("Stage2");
        // }
        on =swich.Swich_;
        if (block == null && on == false)
        {
            
            if (stop == true)
            {
                Debug.Log("ほい");
                Instantiate(blockprefab, this.transform.position, Quaternion.identity);
                stop = false;
            }
        }
    }
}
