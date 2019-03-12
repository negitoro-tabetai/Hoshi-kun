using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(MovableBlock))]
public class Enemy_Knight : Enemy
{
    //----------------------------------------------------------
    //変数宣言
    [SerializeField] float _findRayLength;
    [SerializeField] LayerMask _playerLayer;
    [SerializeField] float k_speed;
    //----------------------------------------------------------

    protected override void Start()
    {
        base.Start();
    }


    new void Update()
    {
        //向いている方向にプレイヤーがいた場合、且つ、地面と接している場合
        if (Physics2D.BoxCast(transform.position, transform.localScale, 0, transform.right, _findRayLength, _playerLayer) && 
            Physics2D.Raycast(transform.position + transform.right, Vector2.down, _rayLength, _groundLayer))
        {
            //_isTouching = true;
            LookAtPlayer();
            Move();
        }
        else
        {
            base.Update();
        }
    }


    // Update is called once per frame
    new void Move()
    {
        //Vector3 target = (_player.transform.position - transform.position).normalized;
        _rigidbody.velocity = new Vector2(transform.right.x * k_speed, _rigidbody.velocity.y);
    }
}
