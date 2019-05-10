using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_King : Enemy_Knight
{

    //-----------------------------------------------
    //変数宣言
    [SerializeField, Tooltip("ジャンプする高さ")] float _jumpPower;
    [SerializeField] ContactFilter2D contactfilter;
    //-----------------------------------------------


    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }


    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (_rigidbody.IsTouching(contactfilter))
        {
            _damage = true;
            Damage();
        }
    }


    protected override void Move()
    {
        _time += Time.deltaTime;
        if (IsGround())
        {
            if (_time > _moveTime || !CanMoveForward())
            {
                Direction();
                _time = 0;
            }
            _rigidbody.velocity = new Vector2(transform.right.x * _speed, _jumpPower);
        }
    }


    protected override void Move(float speed, float moveTime)
    {
        _time += Time.deltaTime;
        if (IsGround())
        {
            if (_time > moveTime || !CanMoveForward())
            {
                Direction();
                _time = 0;
            }
            _rigidbody.velocity = new Vector2(transform.right.x * speed, _jumpPower);
        }
    }


    protected override void Follow()
    {
        if (IsGround())
        {
            if (!CanMoveForward())
            {
                Direction();
            }
            _rigidbody.velocity = new Vector2(transform.right.x * _followSpeed, _jumpPower);
        }
    }


    protected override void DamageMove()
    {
        Move(_afterSpeed, _afterMoveTime);
    }
}
