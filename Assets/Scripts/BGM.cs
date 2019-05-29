using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGM : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Title")
        {
            AudioManager.Instance.PlayBGM("Title");
        }
        else
        {
            AudioManager.Instance.PlayBGM("Stage");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
