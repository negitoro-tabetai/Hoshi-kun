using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolution_Player : MonoBehaviour
{
    [SerializeField]public GameObject[] Object;

  
    [SerializeField] int block;
   
    public int Block
    {
        get
        {
            return block;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void AddBlock()
    {
        block++;
    }
    public void takeBlock()
    {
        block--;
    }

    public void Throw_()
    {
        if (block == 1)
        {
            Object[1].GetComponent<Revolution>().Throw(); Debug.Log("1つめ");
        }

        else
            if (block == 2)
        {
            Object[2].GetComponent<Revolution>().Throw(); Debug.Log("2つめ");
        }
        else
        if (block == 3)
        {
            Object[3].GetComponent<Revolution>().Throw();
            Debug.Log("３つめ");
        }





        //switch (block)
        //{
        //    case 1:
        //        Object[1].GetComponent<Revolution>().Throw();
        //        break;
        //    case 2:
        //        Object[2].GetComponent<Revolution>().Throw();
        //        break;

        //    case 3:
        //        Object[3].GetComponent<Revolution>().Throw();
        //        Debug.Log("aaaa");
        //        break;
        //}
    }

public void Animation()
    {
        if (block == 1)
        {
            Object[1].GetComponent<Animation>().Play("block"); Debug.Log("アニメ１");
        }
        else
            if (block == 2)
        {
            Object[2].GetComponent<Animation>().Play("block 2"); Debug.Log("アニメ2");
        }
        else
        if (block == 3)
        {
            Object[3].GetComponent<Animation>().Play("block 3"); Debug.Log("アニメ3");
        }
    }

}
