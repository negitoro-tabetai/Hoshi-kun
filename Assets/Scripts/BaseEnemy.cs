using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    //--------------------------------------------------------------------------------
    //変数宣言

    //プレイヤーと接触したか
    protected Rigidbody2D _rigidbody;
    [SerializeField] protected GameObject _player;
    [SerializeField, Tooltip("Destroy時に出現するエフェクト")] protected GameObject _destroyEffect;  
    [SerializeField, Tooltip("地面のレイヤーマスク")] protected LayerMask _groundLayer;
    [SerializeField, Tooltip("rayの長さ")] protected float _rayLength = 0.6f;
    //[SerializeField] protected float _pinchRayLength = 0.5f;
    [SerializeField, Tooltip("移動する制限時間")] protected float _moveTime;
    [SerializeField, Tooltip("移動の速さ")] protected float _speed;
    [SerializeField, Tooltip("移動後の経過時間")] protected float _time = 0;
    [SerializeField, Tooltip("ライフポイント")] int _life;
    [SerializeField, Tooltip("攻撃力")] int _attackPoint;
    protected bool _isTouching;　//プレイヤーに触れたか判定する変数
    protected Animator _animator;
    MovableBlock _movable;
    //回転する角度
    const int _rotationAngle = 180;
    //--------------------------------------------------------------------------------


    public int AttackPoint
    {
        get
        {
            return _attackPoint;
        }
    }

    /// <summary>
    /// 接地してるか確認する関数
    /// </summary>
    /// <returns>下にrayを打った結果</returns>
    protected bool IsGround()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, _rayLength, _groundLayer);
    }


    /// <summary>
    /// 前に道が続いているか確認する関数
    /// </summary>
    /// <returns>自分の目の前から下に向けてrayを打った結果</returns>
    protected bool CanMoveForward()
    {
        return Physics2D.Raycast(transform.position + transform.right, Vector2.down, _rayLength, _groundLayer);
    }


    ///// <summary>
    ///// 挟まれたか確認する関数
    ///// </summary>
    //protected bool PinchCheck()
    //{
    //    //左右にrayを打って地面のレイヤーを持つオブジェクトに当たったらダメージを受ける
    //    return Physics2D.Raycast(transform.position, Vector2.left, _pinchRayLength, _groundLayer) &&
    //            Physics2D.Raycast(transform.position, Vector2.right, _pinchRayLength, _groundLayer);
    //}


    // Start is called before the first frame update
    protected void Start()
    {
        _movable = GetComponent<MovableBlock>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }
    

    /// <summary>
    /// 移動処理
    /// 地面に着いている場合に向いている方向に進む。
    /// </summary>
    protected virtual void Move()
    {
        if (IsGround())
        {
            _rigidbody.velocity = new Vector2(transform.right.x * _speed, _rigidbody.velocity.y);
            // 時間を計測し、進む方向を変える関数を呼び出す
            _time += Time.deltaTime;
            if (_time > _moveTime || !CanMoveForward())
            {
                Direction();
                _time = 0;
            }
        }
    }
    protected virtual void Move(float speed, float moveTime)
    {
        if (IsGround())
        {
            _rigidbody.velocity = new Vector2(transform.right.x * speed, _rigidbody.velocity.y);
            _time += Time.deltaTime;
            if (_time > moveTime || !CanMoveForward())
            {
                Direction();
                _time = 0;
            }
        }
    }


    /// <summary>
    /// ダメージ処理
    /// </summary>
    protected virtual void Damage()
    {
        if (_life >= 0)
        {
            _life--;
        }
        if (_life == 0)
        {
            destroy();
        }
    }


    public void destroy()
    {
        Instantiate(_destroyEffect, transform.position, Quaternion.identity);
        AudioManager.Instance.PlaySE("Die");

        Destroy(gameObject);
    }


    /// <summary>
    /// オブジェクトの方向転換
    /// </summary>
    protected void Direction()
    {
        //真下に打ったレイが当たっている場合、方向転換をする
        if (IsGround())
        {
            transform.Rotate(0, _rotationAngle, 0);
        }
    }


    /// <summary>
    /// プレイヤーの方向を向く
    /// </summary>
    protected void LookAtPlayer()
    {
        //回転軸をyだけに制限し
        //ターゲットの方向に方向転換する
        Vector3 playerDirection = _player.transform.position;
        playerDirection.y = transform.position.y;
        transform.right = playerDirection - transform.position;
    }


    protected void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            LookAtPlayer();
            _isTouching = true;
        }

    }


    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Field")
        {
            if (other.transform.parent.GetComponent<Player>().IsUsingGravity)
            {
                enabled = false;
                _movable.enabled = true;
            }
            else
            {
                enabled = true;
                _movable.enabled = false;
            }
        }

    }


    protected virtual void OnTriggerEnter2D(Collider2D other )
    {
        //公転で飛ばされたブロックに当たった場合ダメージを受ける
        if (other.gameObject.tag == "RevolutionBlock")
        {
            AudioManager.Instance.PlaySE("Damage");

            Damage();
        }
    }

}
