using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    [SerializeField, Tooltip("始点")] Transform _start;
    [SerializeField, Tooltip("終点")] Transform _end;
    [SerializeField, Tooltip("スピード")] float _speed;
    [SerializeField] ContactFilter2D _contactFilter2D;

    LineRenderer _lineRenderer;
    GravitySource _gravitySource;
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

        if (_isMoving && !_isTouching)
        {
            transform.position = Vector3.MoveTowards(transform.position, NearestPosition(_start.position, _end.position, _gravitySource.transform.position), step);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, _start.position, step);
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Field")
        {

            _gravitySource = other.transform.root.GetComponent<GravitySource>();

            if (_gravitySource)
            {
                if (_gravitySource.IsUsingGravity)
                {
                    _isMoving = true;
                }
                else
                {
                    _isMoving = false;
                }
            }
        }
    }

    Vector3 NearestPosition(Vector3 a, Vector3 b, Vector3 p)
    {
        Vector3 ab = b - a;
        Vector3 ap = p - a;

        float r = Vector3.Dot(ab, ap) / Vector3.Dot(ab, ab);

        if (r <= 0)
        {
            return a;
        }
        else if (r >= 1)
        {
            return b;
        }
        else
        {
            return a + r * ab;
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
