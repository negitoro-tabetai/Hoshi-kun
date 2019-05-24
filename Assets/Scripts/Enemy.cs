using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BaseEnemy
{
    //----------------------------------------------------------------------------------
    //変数宣言
    
    //プレイヤーとの距離
    Vector2 _playerToDistance;
    //プレイヤーと接触した後通常に戻るためのY座標
    const float _distanceLimit_X = 5;
    //プレイヤーと接触した後通常に戻るためのY座標
    const float _distanceLimit_Y = 3;
    //----------------------------------------------------------------------------------

    
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        _animator = GetComponentInChildren<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        _animator.SetBool("isWalk", true);
        TouchCheck();
    }


    protected void TouchCheck()
    {
        if (_isTouching)
        {
            Stop();
            //プレイヤーとの距離
            _playerToDistance = _player.transform.position - transform.position;

            //接触してから一定距離離れた場合通常に戻る
            if (Mathf.Abs(_playerToDistance.x) >= _distanceLimit_X ||
                Mathf.Abs(_playerToDistance.y) >= _distanceLimit_Y)
            {
                _isTouching = false;
            }
        }
        else
        {
            Move();
        }
    }
    /// <summary>
    /// 敵の動きを止める関数
    /// </summary>
    protected void Stop()
    {
        //プレイヤーの方向を向く関数を呼び出す
        LookAtPlayer();
        _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
        _animator.SetBool("isWalk", false);
    }
}
