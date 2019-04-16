using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGround : MonoBehaviour
{
    Collider2D _collider;
    bool _isTouching;
    [SerializeField, Tooltip("上方向フィルター")] ContactFilter2D _filter;

    void Start()
    {
        _collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (Physics2D.IsTouching(_collider, _filter))
        {
            
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && Physics2D.IsTouching(_collider, _filter))
        {
            _isTouching = true;
            other.transform.SetParent(transform);
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _isTouching = false;
            other.transform.SetParent(null);
        }
    }
}
