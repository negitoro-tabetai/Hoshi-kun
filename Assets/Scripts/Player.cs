using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D _rigidbody;
    Collider2D _collider;

    [SerializeField, Tooltip("体力")] int _hp;
    [SerializeField, Tooltip("移動速度")] float _moveSpeed;
    [SerializeField, Tooltip("走る速度")] float _runSpeed;
    [SerializeField, Tooltip("無敵時間")] float _invincibleTime;
    [SerializeField, Tooltip("硬直時間")] float _stunningTime;
    [SerializeField, Tooltip("ジャンプの強さ")] float _jumpForce;
    [SerializeField, Tooltip("ノックバックの強さ")] float _knockBackForce;
    // [SerializeField, Tooltip("接地判定するレイヤー")] LayerMask _groundLayer;
    // [SerializeField, Tooltip("Rayの長さ")] float _rayLength;
    // [SerializeField, Tooltip("Rayの飛ばす範囲")] float _width;
    [SerializeField, Tooltip("判定用フィルター")] ContactFilter2D _upFilter, _downFilter;
    [SerializeField, Tooltip("引力の範囲の表示")] GameObject _field;
    // 左右入力
    float _inputX;
    // 走っているか
    bool _isRunning;
    bool _isMoving;
    // 硬直しているか
    bool _isStunning;

    Vector3 _previousPosition;

    // Layer名
    const string PlayerLayer = "Player";
    const string InvincibleLayer = "InvinciblePlayer";

    const int MaxHP = 100;

    public int Hp
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = value;
            _hp = Mathf.Clamp(_hp, 0, MaxHP);
        }
    }

    // 無敵か
    public bool IsInvincible { get; set; }
    // 引力を使用しているか
    public bool IsUsingGravity { get; set; }

    public bool IsRunning
    {
        get
        {
            return _isRunning && _isMoving;
        }
    }
    public float InputX
    {
        get
        {
            return _inputX;
        }
    }

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();

        // リスポーン地点から
        if (GameManager.Instance.RespawnPoint != Vector3.zero)
        {
            transform.position = GameManager.Instance.RespawnPoint;
        }
    }

    void Update()
    {
        // LeftShiftキーで走る
        if (Input.GetButton("Run"))
        {
            _isRunning = true;
        }
        else
        {
            _isRunning = false;
        }

        if (Physics2D.IsTouching(_collider, _downFilter))
        {
            // Vキーで引力使用
            if (Input.GetButtonDown("UseGravity"))
            {
                IsUsingGravity = true;
                _rigidbody.isKinematic = true;
            }
            if (Input.GetButtonUp("UseGravity"))
            {
                IsUsingGravity = false;
                _rigidbody.isKinematic = false;
            }
        }
        else
        {
            _rigidbody.isKinematic = false;
            IsUsingGravity = false;
        }

        if (Input.GetButtonDown("Jump") && Physics2D.IsTouching(_collider, _downFilter) && !Physics2D.IsTouching(_collider, _upFilter) && !_isStunning && !IsUsingGravity)
        {
            Jump();
        }
        // 左右入力
        if (IsUsingGravity)
        {
            _inputX = 0;
        }
        else
        {
            _inputX = System.Math.Sign(Input.GetAxisRaw("Horizontal"));
        }

        // 範囲表示
        if (IsUsingGravity)
        {
            _field.SetActive(true);
        }
        else
        {
            _field.SetActive(false);
        }
        _isMoving = _previousPosition.x != transform.position.x;
    }

    void FixedUpdate()
    {
        if (!_isStunning)
        {
            if (_isRunning)
            {
                _rigidbody.velocity = new Vector3(_inputX * _runSpeed, _rigidbody.velocity.y, 0);
            }
            else
            {
                _rigidbody.velocity = new Vector3(_inputX * _moveSpeed, _rigidbody.velocity.y, 0);
            }
        }
        _previousPosition = transform.position;
    }

    void Jump()
    {
        _rigidbody.AddForce((Vector2.up) * _jumpForce, ForceMode2D.Impulse);
    }

    /// <param name="attackPoint">ダメージ量</param>
    /// <param name="enemyPosition">衝突した敵の位置</param>
    public void Damage(int attackPoint, Vector3 enemyPosition)
    {
        if (!IsInvincible)
        {
            IsUsingGravity = false;
            _rigidbody.isKinematic = false;
            _rigidbody.velocity = Vector3.zero;
            const float VerticalForce = 2;

            // ダメージ処理
            Hp -= attackPoint;
            if (Hp == 0)
            {
                GameManager.Instance.ReroadScene();
            }

            //自分の位置と敵の位置を比較して方向を決定
            Vector3 direction = new Vector3(transform.position.x - enemyPosition.x, 0, 0).normalized;
            direction = new Vector3(direction.x, VerticalForce, 0).normalized;

            // ノックバック
            _rigidbody.AddForce(direction * _knockBackForce, ForceMode2D.Impulse);

            // 硬直と無敵化
            StartCoroutine(Stun());
            StartCoroutine(BecomesInvincible());
        }
    }

    /// <param name="healPoint">回復量</param>
    public void Heal(int healPoint)
    {
        Hp += healPoint;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy" && !IsInvincible)
        {
            Damage(other.gameObject.GetComponent<BaseEnemy>().AttackPoint, other.transform.transform.position);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "HealItem")
        {
            Heal(other.GetComponent<HealItem>().HealPoint);
            Destroy(other.gameObject);
        }
    }

    /// <summary>
    /// 硬直するコルーチン
    /// </summary>
    IEnumerator Stun()
    {
        _isStunning = true;
        yield return new WaitForSeconds(_stunningTime);
        _isStunning = false;
    }

    /// <summary>
    /// 無敵になるコルーチン
    /// </summary>
    IEnumerator BecomesInvincible()
    {
        IsInvincible = true;
        gameObject.layer = LayerMask.NameToLayer(InvincibleLayer);
        yield return new WaitForSeconds(_invincibleTime);
        gameObject.layer = LayerMask.NameToLayer(PlayerLayer);
        IsInvincible = false;
    }
}
