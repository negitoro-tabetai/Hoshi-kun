﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(MovableBlock))]
public class Enemy_Knight : Enemy
{
    [SerializeField] float _findRayLength;
    [SerializeField] LayerMask _playerLayer;
    [SerializeField] float k_speed;


    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        Debug.DrawRay(transform.position, transform.right * _findRayLength, Color.red);
        if (!IsTouching && Physics2D.Raycast(transform.position, transform.right, _findRayLength, _playerLayer))
        {
            
            Debug.Log("レイが当たった且つプレイヤに触れてない");
            Move();
        }
        else
        {
            Debug.Log("");
            base.Move();
            base.Update();
        }
    }


    // Update is called once per frame
    protected override void Move()
    {
        Vector3 target = (_player.transform.position - transform.position).normalized;
        target.y = transform.position.y;
        //_rigidbody.velocity = new Vector2(target.x * k_speed, _rigidbody.velocity.y);
        transform.Translate(target * k_speed * Time.deltaTime);
    }
}