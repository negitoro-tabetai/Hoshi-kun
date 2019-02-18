using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody _rigidbody;
    [SerializeField] float _jumpForce;
    [SerializeField] float _moveSpeed;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] float _rayLength;
    [SerializeField] float _radius;
    float _inputX;

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

        _inputX = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        _rigidbody.velocity = new Vector3(_inputX * _moveSpeed, _rigidbody.velocity.y, _rigidbody.velocity.z);
    }

    void Jump()
    {
        _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    /// <returns>地面についているか</returns>
    bool IsGrounded()
    {
        Ray rayRight = new Ray(transform.position + new Vector3(_radius, 0, 0), Vector3.down);
        Ray rayLeft = new Ray(transform.position + new Vector3(-_radius, 0, 0), Vector3.down);

        bool isGrounded = Physics.Raycast(rayLeft, _rayLength, _groundLayer) || Physics.Raycast(rayRight, _rayLength, _groundLayer);

        return isGrounded;
    }
}