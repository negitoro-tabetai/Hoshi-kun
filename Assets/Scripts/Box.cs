using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Box : MonoBehaviour
{
    [SerializeField] GameObject Text;
    // Start is called before the first frame update
    void Start()
    {
        Text.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            Debug.Log("到達");
            GameObject.Find("Player").GetComponent<Player>().enabled=false;
            Goal();
        }
    }

    void Goal()
    {
        Text.SetActive(true);
        Text.GetComponent<TextMeshProUGUI>().text = "Game Clear!!";
        Debug.Log("テキスト表示");
        Invoke("Load", 3.0f);
    }

   void Load()
    {
        GameManager.Instance.ResetPoint();
        SceneManager.LoadScene("Title");
    }

}
