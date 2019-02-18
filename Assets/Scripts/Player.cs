using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody _rigidbody;
    [SerializeField] float _jumpForce;
    [SerializeField] float _moveSpeed;
    float _inputX;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
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
}
