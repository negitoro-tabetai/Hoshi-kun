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

    void OntriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            Load();
        }
    }

    IEnumerator Load()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(nextScene);
    }

}
