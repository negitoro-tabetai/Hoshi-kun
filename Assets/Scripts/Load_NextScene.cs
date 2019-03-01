using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Load_NextScene : MonoBehaviour
{
    public SceneObject nextScene;

    private void Update()
    {

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    SceneManager.LoadScene(nextScene);
        //}

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            StartCoroutine(Load());
        }
    }

    IEnumerator Load()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("A");
        SceneManager.LoadScene(nextScene);
    }

}
