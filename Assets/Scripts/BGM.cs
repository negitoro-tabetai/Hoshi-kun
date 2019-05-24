using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGM : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "1-1")
        {
            AudioManager.Instance.PlayBGM("Stage");
        }
        else if (SceneManager.GetActiveScene().name == "Title")
        {
            AudioManager.Instance.PlayBGM("Title");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
