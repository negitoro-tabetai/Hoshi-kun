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
    [SerializeField, Tooltip("地面のレイヤーマスク")] protected LayerMask _groundLayer;
    [SerializeField, Tooltip("rayの長さ")] protected float _rayLength = 0.6f;
    [SerializeField, Tooltip("移動する制限時間")] protected float _moveTime;
    [SerializeField, Tooltip("移動の速さ")] protected float _speed;
    [SerializeField, Tooltip("移動後の経過時間")] float _time = 0;
    [SerializeField, Tooltip("ライフポイント")] int _life;
    [SerializeField, Tooltip("攻撃力")] int _attackPoint;
    protected bool _isTouching;
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

    // Start is called before the first frame update
    protected void Start()
    {
        _movable = GetComponent<MovableBlock>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    
    /// <summary>
    /// 挟まれたか確認する関数
    /// </summary>
    protected void PinchCheck()
    {
        //左右にrayを打って地面のレイヤーを持つオブジェクトに当たったらダメージを受ける
        if (Physics2D.Raycast(transform.position, Vector2.left / 2, _rayLength, _groundLayer) &&
            Physics2D.Raycast(transform.position, Vector2.right / 2, _rayLength, _groundLayer))
        {
            Damage();
        }

    }



    /// <summary>
    /// 移動処理
    /// </summary>
    protected void Move()
    {
        if (IsGround())
        {
            _rigidbody.velocity = new Vector2(transform.right.x * _speed, _rigidbody.velocity.y);
            _time += Time.deltaTime;
            if (_time > _moveTime || !CanMoveForward())
            {
                Direction();
                _time = 0;
            }
        }
    }
    protected void Move(float speed, float moveTime)
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
    void Damage()
    {
        if (_life >= 0)
        {
            _life--;
        }
        if (_life == 0)
        {
            Destroy(this.gameObject);
        }
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


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
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

    protected void OnTriggerEnter2D(Collider2D other)
    {
        //公転で飛ばされたブロックに当たった場合ダメージを受ける
        if (other.gameObject.tag == "RevolutionBlock")
        {
            Damage();
        }
        
    }
}
