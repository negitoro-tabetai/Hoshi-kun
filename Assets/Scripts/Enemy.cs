using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //----------------------------------------------------------------------------------
    //変数宣言
    [SerializeField, Tooltip("移動後の経過時間")] float _time = 0;
    [SerializeField, Tooltip("移動する制限時間")] float _moveTime;
    [SerializeField, Tooltip("移動の速さ")] float _speed;
    [SerializeField, Tooltip("ライフポイント")] int _life;
    [SerializeField, Tooltip("プレイヤーと接触した後通常に戻るための距離")] float _distanceLimit = 5;
    [SerializeField, Tooltip("地面のレイヤーマスク")] LayerMask _groundLayer;
    [SerializeField, Tooltip("攻撃力")] int _attackPoint;
    [SerializeField, Tooltip("rayの長さ")] float _rayLength = 0.6f;
    protected bool _isTouching;
    protected Rigidbody2D _rigidbody;
    [SerializeField] protected GameObject _player;
    MovableBlock _movable;
    //プレイヤーとの距離
    Vector2 _playerToDistance;
    //回転する角度
    const int _rotationAngle = 180;
    //プレイヤーと接触したか
    //----------------------------------------------------------------------------------



    public int AttackPoint
    {
        get
        {
            return _attackPoint;
        }
    }

    public bool IsTouching
    {
        get
        {
            return _isTouching;
        }
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _movable = GetComponent<MovableBlock>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Move();
        if (!_isTouching)
        {
            MoveTimer();
        }
        else
        {
            Direction();
            _playerToDistance = _player.transform.position - transform.position;

            //接触してから一定距離離れた場合通常に戻る
            if(Mathf.Abs(_playerToDistance.x) >= _distanceLimit)
            {
                _isTouching = false;
            }
        }

        //左右にrayを打って地面のレイヤーを持つオブジェクトに当たったらダメージを受ける
        if(Physics2D.Raycast(transform.position , Vector2.left / 2, _rayLength, _groundLayer) &&
            Physics2D.Raycast(transform.position, Vector2.right / 2, _rayLength, _groundLayer))
        {
            Damage();
        }
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    protected virtual void Move()
    {
        if (!_isTouching &&
            Physics2D.Raycast(transform.position, Vector2.down, _rayLength, _groundLayer))
        {
            
            transform.Translate(Vector2.right * _speed * Time.deltaTime);
        }
    }
    
    /// <summary>
    /// 方向転換するまでの時間計測
    /// </summary>
    void MoveTimer()
    {
        _time += Time.deltaTime;
        if(_time > _moveTime || !Physics2D.Raycast(transform.position + transform.right, Vector2.down,_rayLength, _groundLayer))
        {
            Direction();
            _time = 0;
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
    void Direction()
    {
        if (_isTouching)
        {
            //回転軸をyだけに制限し
            //ターゲットの方向に方向転換する
            Vector3 target = _player.transform.position;
            target.y = transform.position.y;
            transform.right = target - transform.position;
        }
        else if(!_isTouching && 
            Physics2D.Raycast(transform.position, Vector2.down, _rayLength, _groundLayer))
        {
            transform.Rotate(0, _rotationAngle, 0);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
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

        //公転で飛ばされたブロックに当たった場合ダメージを受ける
        if (other.gameObject.tag == "RevolutionBlock")
        {
            Damage();
        }
    }
}
