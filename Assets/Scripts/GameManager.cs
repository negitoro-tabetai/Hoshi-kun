using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public bool IsPausing { get; set; }
    public bool IsGoal { get; set; }
    public Vector3 RespawnPoint { get; set; }
    new void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void ReroadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ResetPoint()
    {
        RespawnPoint = Vector3.zero;
    }
}
