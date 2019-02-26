using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableBlock : MonoBehaviour
{
    //プレイヤー
    Player _player;
    //移動スピード
    [SerializeField] float _speed;
    [SerializeField, Tooltip("接地判定するレイヤー")] LayerMask _groundLayer;
    [SerializeField, Tooltip("Rayの長さ")] float _rayLength;
    [SerializeField, Tooltip("Rayの飛ばす範囲")] float _width;

    Rigidbody2D _rigidbody;
    Vector3 _velocity;
    Vector3 _gravity;
    bool _isMoving;
    bool _isTouching;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        if (_rigidbody.isKinematic)
        {
            if (_isMoving)
            {
                _gravity = new Vector3((_player.transform.position - transform.position).normalized.x, 0, 0) * _speed * Time.fixedDeltaTime;
            }
            else
            {
                _gravity = Vector2.zero;
            }

            if (IsGrounded())
            {
                _velocity = Vector2.zero;
            }
            else
            {
                _velocity += new Vector3(0, Physics2D.gravity.y, 0) * Time.fixedDeltaTime;
                Debug.Log("!!!");
            }

            _rigidbody.MovePosition(transform.position + _gravity + _velocity * Time.fixedDeltaTime);
        }
        else
        {
            if (_isMoving)
            {
                if (IsGrounded())
                    _rigidbody.velocity = new Vector3((_player.transform.position - transform.position).normalized.x * _speed, 0, 0);
                else
                {
                    _rigidbody.velocity = new Vector3((_player.transform.position - transform.position).normalized.x * _speed, _rigidbody.velocity.y, 0);
                }
            }
            else
            {
                if (IsGrounded())
                    _rigidbody.velocity = Vector3.zero;
                else
                {
                    _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
                }
            }
            _velocity = _rigidbody.velocity;
            _velocity.x = 0;
        }
    }

    /// <summary>
    /// 左端と右端に下向きのRayを飛ばし、片方でも地面に当たればtrueを返す
    /// </summary>
    /// <returns>地面についているか</returns>
    bool IsGrounded()
    {
        bool isGrounded = Physics2D.Raycast(transform.position + new Vector3(_width, 0, 0), Vector3.down, _rayLength, _groundLayer) || Physics2D.Raycast(transform.position + new Vector3(-_width, 0, 0), Vector3.down, _rayLength, _groundLayer);
        return isGrounded;
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Field")
        {
            if (!_player)
            {
                _player = other.transform.root.GetComponent<Player>();
            }

            if (_player)
            {
                if (IsGrounded() && !_player.IsMoving && !_isTouching)
                {
                    _rigidbody.isKinematic = false;
                }
                else
                {
                    _rigidbody.isKinematic = true;
                }

                if (_player.IsUsingGravity && !_isTouching)
                {
                    _isMoving = true;
                    _rigidbody.isKinematic = false;
                }
                else
                {
                    _isMoving = false;
                }
            }
        }

    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _isTouching = true;
            Debug.Log("!");
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _isTouching = false;
            Debug.Log("?");
        }
    }
}
