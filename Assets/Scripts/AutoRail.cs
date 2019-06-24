using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRail : MonoBehaviour
{
    [SerializeField, Tooltip("始点")] Transform _start;
    [SerializeField, Tooltip("終点")] Transform _end;
    [SerializeField, Tooltip("スピード")] float _speed;
    [SerializeField] ContactFilter2D _contactFilter2D;

    Rigidbody2D _rigidbody2D;
    LineRenderer _lineRenderer;
    Player _player;
    bool _isMoving;
    bool _isTouching;

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
      float t = Mathf.PingPong(Time.time * _speed/ Vector3.Distance(_start.position, _end.position), 1);
      transform.position = Vector3.Lerp(_start.position,_end.position,t);
    }
}
