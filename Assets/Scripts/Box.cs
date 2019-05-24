using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Box : MonoBehaviour
{
    [SerializeField] GameObject Text;
    Animator _boxAnimator;
    Animator _playerAnimator;
    // Start is called before the first frame update
    void Start()
    {
        Text.SetActive(false);
        _boxAnimator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsGoal && Input.GetButtonDown("Jump"))
        {
            Load();
        }
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            GameManager.Instance.IsGoal = true;
            col.GetComponent<Player>().enabled=false;
            _playerAnimator = col.GetComponentInChildren<Animator>();
            _playerAnimator.transform.localRotation = Quaternion.Euler(0, 180, 0);
            Invoke("Goal", 0.5f);
        }
    }

    void Goal()
    {
        GameObject.Find("BGM").SetActive(false);
        
        AudioManager.Instance.PlayBGM("Clear");

        _playerAnimator.SetBool("Clear", true);
            Text.SetActive(true);
            _boxAnimator.SetTrigger("Open");
    }
   void Load()
    {
        GameManager.Instance.ResetPoint();
        GameManager.Instance.IsGoal = false;
        FadeManager.Instance.SceneFade("Title", 2);
    }

}
