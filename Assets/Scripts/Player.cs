using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody _rigidbody;
    // 移動速度
    [SerializeField] float _moveSpeed;
    // 走る速度
    [SerializeField] float _runSpeed;
    // ジャンプの強さ
    [SerializeField] float _jumpForce;
    //接地判定するレイヤー
    [SerializeField] LayerMask _groundLayer;
    // Rayの長さ
    [SerializeField] float _rayLength;
    // Rayの飛ばす範囲
    [SerializeField] float _width;
    
    float _inputX;
    bool _isRunning;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jump();
        }

        // LeftShiftキーで走る
        if (Input.GetButton("Fire3"))
        {
            _isRunning = true;
        }
        else
        {
            _isRunning = false;
        }

        _inputX = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        if  (_isRunning)
        {
            _rigidbody.velocity = new Vector3(_inputX * _runSpeed, _rigidbody.velocity.y, 0);
        }
        else
        {
            _rigidbody.velocity = new Vector3(_inputX * _moveSpeed, _rigidbody.velocity.y, 0);
        }
    }

    void Jump()
    {
        _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
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
}