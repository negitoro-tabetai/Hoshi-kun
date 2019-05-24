using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathArea : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            AudioManager.Instance.PlaySE("Die");

            FadeManager.Instance.SceneFade(SceneManager.GetActiveScene().name, 0.2f);
        }
    }
}
