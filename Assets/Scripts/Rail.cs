using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    [SerializeField, Tooltip("始点")] Transform _start;
    [SerializeField, Tooltip("終点")] Transform _end;
    [SerializeField, Tooltip("スピード")] float _speed;
    Player _player;
    bool _isMoving;
    Rigidbody2D _rigidbody2D;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Field")
        {
            if (!_player)
            {
                _player = other.transform.root.GetComponent<Player>();
            }
            Debug.Log(_player.IsUsingGravity);
            if (_player)
            {
                if (_player.IsUsingGravity)
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
    void FixedUpdate()
    {
        //オブジェクトの移動
        float step = _speed * Time.fixedDeltaTime;
        if (_isMoving)
        {
            Vector3 targetPosition = Vector3.Lerp(_start.position, _end.position, (_start.position.y - _player.transform.position.y) / (_start.position.y - _end.position.y));
            _rigidbody2D.MovePosition(Vector3.MoveTowards(transform.position, targetPosition, step));
        }
        else
        {
            _rigidbody2D.MovePosition(Vector3.MoveTowards(transform.position, _start.position, step));
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(_start.position, _end.position);
    }
}
