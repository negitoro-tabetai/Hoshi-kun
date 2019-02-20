using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableBlock : MonoBehaviour
{
    //プレイヤー
    Transform _player;
    //移動スピード
    [SerializeField] float _speed;
    [SerializeField, Tooltip("接地判定するレイヤー")] LayerMask _groundLayer;
    [SerializeField, Tooltip("Rayの長さ")] float _rayLength;
    [SerializeField, Tooltip("Rayの飛ばす範囲")] float _width;

    Rigidbody _rigidbody;
    bool _isMoveing;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        if (_isMoveing)
        {
            if (IsGrounded())
            {
                _rigidbody.velocity = (_player.position - transform.position).normalized * _speed;
            }
            else
            {
                _rigidbody.velocity += Physics.gravity * Time.fixedDeltaTime;
            }
        }
        else
        {
            _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
        }
    }

    /// <summary>
    /// 左端と右端に下向きのRayを飛ばし、片方でも地面に当たればtrueを返す
    /// </summary>
    /// <returns>地面についているか</returns>
    bool IsGrounded()
    {
        Ray rayRight = new Ray(transform.position + new Vector3(_width, 0, 0), Vector3.down);
        Ray rayLeft = new Ray(transform.position + new Vector3(-_width, 0, 0), Vector3.down);

        bool isGrounded = Physics.Raycast(rayLeft, _rayLength, _groundLayer) || Physics.Raycast(rayRight, _rayLength, _groundLayer);

        return isGrounded;
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Field")
        {
            if (!_player)
            {
                _player = other.transform.parent;
            }
            if (_player.GetComponent<Player>().IsUsingGravity)
            {
                _isMoveing = true;
                Debug.Log("!");
            }
            else
            {
                _isMoveing = false;
                Debug.Log("?");
            }
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" && IsGrounded())
        {
            _rigidbody.isKinematic = true;
        }
    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            _rigidbody.isKinematic = false;
        }
    }
}
