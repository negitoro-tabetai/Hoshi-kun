using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody _rigidbody;

    [SerializeField, Tooltip("移動後の経過時間")] float _time = 0;
    [SerializeField, Tooltip("移動する制限時間")] float _moveTime;
    [SerializeField, Tooltip("移動の速さ")] float _speed;
    [SerializeField, Tooltip("ライフポイント")] int _life;
    [SerializeField, Tooltip("地面のレイヤーマスク")] LayerMask _groundLayer;
    [SerializeField, Tooltip("攻撃力")] int _attackPoint;
    [SerializeField, Tooltip("rayの長さ")] float _rayLength;
    [SerializeField] GameObject _player;
   
    //回転する角度
    const int _rotationAngle = 180;
    //プレイヤーと接触したか
    bool _isTouching;
    //プレイヤー
    Vector2 _playerToDirection;


    public int AttackPoint
    {
        get
        {
            return _attackPoint;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isTouching)
        {
            MoveTimer();
        }
        Damage();
        Debug.DrawRay(transform.position + transform.right, Vector2.down, Color.red, Time.deltaTime);
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    void FixedUpdate()
    {
        if (_isTouching)
        {
            
            _rigidbody.velocity = new Vector2(_playerToDirection.y * _speed, _rigidbody.velocity.y);
        }
        else
        {
            _rigidbody.velocity = new Vector2(transform.right.x * _speed, _rigidbody.velocity.y);
        }
        
    }

    /// <summary>
    /// 方向転換するまでの時間計測
    /// </summary>
    void MoveTimer()
    {
        _time += Time.deltaTime;
        if(_time > _moveTime || 
            !Physics.Raycast(transform.position + transform.right, Vector2.down, _rayLength, _groundLayer))
        {
            Dir();
            _time = 0;
        }
    }
    /// <summary>
    /// ダメージ処理
    /// </summary>
    void Damage()
    {
        if (Input.GetKeyDown(KeyCode.B) && _life != 0)
        {
            _life--;
        }
        if (_life == 0)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// オブジェクトの方向転換
    /// </summary>
    void Dir()
    {
        if (_isTouching)
        {
            //transform.Rotate(0, _playerToDirection.y * _rotationAngle, 0);
        }
        else
        {
            transform.Rotate(0, _rotationAngle, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            _isTouching = true;
        }
    }
}
