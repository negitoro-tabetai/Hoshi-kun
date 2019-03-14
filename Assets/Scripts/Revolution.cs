using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolution : MonoBehaviour
{
    [SerializeField, Tooltip("ほしくん")] GameObject Hoshikun;
    float this_distance;
    float this_distanceY;
    float this_volume;
    float hoshi_volume;

    bool add=true;
    public bool Revolution_On = true;
    

    void Start()
    {

    }

    void Update()
    {
        if (Revolution_On == true)
        {

            //まわせるかどうか判定
            this_distance = transform.position.x - Hoshikun.transform.position.x; //xの差
            this_distance = System.Math.Abs(this_distance);
            this_distanceY = transform.position.y - Hoshikun.transform.position.y;//yの差
            this_distanceY = System.Math.Abs(this_distanceY);



            hoshi_volume = Hoshikun.transform.localScale.x * Hoshikun.transform.localScale.y * Hoshikun.transform.localScale.z;
            this_volume = transform.localScale.x * transform.localScale.y * transform.localScale.z;
            //↑体積


            if (this_distance <= 1.5 && this_volume < hoshi_volume && Hoshikun.GetComponent<Revolution_Player>().RevolutionObject.Count <= 2 &&
            this_distanceY <= 1.5)
            {
                if (add == true)
                {

                    Hoshikun.GetComponent<Revolution_Player>().Object.Add(this.gameObject);
                    add = false;
                }
            }
            else
            {
                add = true;
                Hoshikun.GetComponent<Revolution_Player>().Object.Remove(this.gameObject);

            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //ステージに当たったら消える
        if (col.tag == "Stage")
        {
            Destroy(gameObject);
            Hoshikun.GetComponent<Revolution_Player>().Off();
        }
    }




}