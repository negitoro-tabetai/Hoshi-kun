using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Byon : Enemy
{
    //--------------------------------------
    //変数宣言
    [SerializeField, Tooltip("ジャンプする高さ")] float _jumpPower;
    //--------------------------------------


    new void Start()
    {
        base.Start();
    }


    protected override void Move()
    {
        _time += Time.deltaTime;
        if (IsGround())
        {
            // 時間を計測し、進む方向を変える関数を呼び出す
            if (_time > _moveTime || !CanMoveForward())
            {
                Direction();
                _time = 0;
            }
            _rigidbody.velocity = new Vector2(transform.right.x * _speed, _jumpPower);
        }
    }
}
