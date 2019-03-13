using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    /// <summary>
    /// リスポーン地点を更新する
    /// </summary>
    void UpdateRespawnPoint()
    {
        Debug.Log("リスポーン地点を" + transform.position + "に設定しました。");
        GameManager.Instance.RespawnPoint = transform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            UpdateRespawnPoint();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, transform.localScale.x / 2);
    }
}
