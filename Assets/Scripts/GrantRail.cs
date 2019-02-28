using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrantRail : MonoBehaviour
{
    [SerializeField] GrantBlock _grantBlock;
    [SerializeField, Tooltip("始点")] Transform _start;
    [SerializeField, Tooltip("終点")] Transform _end;
    [SerializeField, Tooltip("スピード")] float _speed;
    [SerializeField] ContactFilter2D _contactFilter2D;

    LineRenderer _lineRenderer;
    bool _isMoving;
    bool _isTouching;
    Rigidbody2D _rigidbody2D;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.startWidth = 0.1f;
        _lineRenderer.endWidth = 0.1f;
        _lineRenderer.positionCount = 2;
        _lineRenderer.useWorldSpace = true;
        _lineRenderer.SetPosition(0, _start.position);
        _lineRenderer.SetPosition(1, _end.position);
    }

    void Update()
    {
        float step = _speed * Time.deltaTime;

        if (_grantBlock.IsOn)
        {
            transform.position = Vector3.MoveTowards(transform.position, _end.position, step);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, _start.position, step);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && _rigidbody2D.IsTouching(_contactFilter2D))
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(_start.position, _end.position);
    }
}
