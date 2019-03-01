using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D _rigidbody2D;

    [SerializeField, Tooltip("体力")] int _hp;
    [SerializeField, Tooltip("移動速度")] float _moveSpeed;
    [SerializeField, Tooltip("走る速度")] float _runSpeed;
    [SerializeField, Tooltip("無敵時間")] float _invincibleTime;
    [SerializeField, Tooltip("硬直時間")] float _stunningTime;
    [SerializeField, Tooltip("ジャンプの強さ")] float _jumpForce;
    [SerializeField, Tooltip("ノックバックの強さ")] float _knockBackForce;
    [SerializeField, Tooltip("接地判定するレイヤー")] LayerMask _groundLayer;
    [SerializeField, Tooltip("Rayの長さ")] float _rayLength;
    [SerializeField, Tooltip("Rayの飛ばす範囲")] float _width;
    [SerializeField, Tooltip("引力の範囲の表示")] GameObject _field;
    // 左右入力
    float _inputX;
    // 走っているか
    bool _isRunning;
    // 無敵か
    bool _isInvincible;
    // 硬直しているか
    bool _isStunning;
    // 引力を使用しているか
    bool _isUsingGravity;

    Vector3 _previousPosition;

    // Layer名
    const string PlayerLayer = "Player";
    const string InvincibleLayer = "InvinciblePlayer";

    public int Hp
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = value;
            _hp = Mathf.Clamp(_hp, 0, 100);
        }
    }

    public bool IsInvincible
    {
        get
        {
            return _isInvincible;
        }
    }

    public bool IsUsingGravity
    {
        get
        {
            return _isUsingGravity;
        }
    }

    public bool IsMoving
    {
        get
        {
            return transform.position != _previousPosition || _inputX != 0;
        }
    }

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
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

        if (IsGrounded())
        {
            // Vキーで引力使用
            if (Input.GetButtonDown("UseGravity"))
            {
                _isUsingGravity = true;
                _rigidbody2D.isKinematic = true;
            }
            if (Input.GetButtonUp("UseGravity"))
            {
                _isUsingGravity = false;
                _rigidbody2D.isKinematic = false;
            }
        }
        else
        {
            _rigidbody2D.isKinematic = false;
            _isUsingGravity = false;
        }

        if (Input.GetButtonDown("Jump") && IsGrounded() && !_isStunning && !_isUsingGravity)
        {
            Jump();
        }
        // 左右入力
        if (_isUsingGravity)
        {
            _inputX = 0;
        }
        else
        {
            _inputX = Input.GetAxisRaw("Horizontal");
        }

        // 範囲表示
        if (_isUsingGravity)
        {
            _field.SetActive(true);
        }
        else
        {
            _field.SetActive(false);
        }

        _previousPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (!_isStunning)
        {
            if (_isRunning)
            {
                _rigidbody2D.velocity = new Vector3(_inputX * _runSpeed, _rigidbody2D.velocity.y, 0);
            }
            else
            {
                _rigidbody2D.velocity = new Vector3(_inputX * _moveSpeed, _rigidbody2D.velocity.y, 0);
            }
        }
    }

    void Jump()
    {
        _rigidbody2D.AddForce((Vector2.up) * _jumpForce, ForceMode2D.Impulse);
    }

    /// <param name="enemyPosition">衝突した敵の位置</param>
    void KnockBack(Vector3 enemyPosition)
    {
        _rigidbody2D.velocity = Vector3.zero;
        const float VerticalForce = 2;

        //自分の位置と敵の位置を比較して方向を決定
        Vector3 direction = new Vector3(transform.position.x - enemyPosition.x, 0, 0).normalized;
        direction = new Vector3(direction.x, VerticalForce, 0).normalized;
        _rigidbody2D.AddForce(direction * _knockBackForce, ForceMode2D.Impulse);
    }

    /// <param name="attackPoint">ダメージ量</param>
    void Damage(int attackPoint)
    {
        Hp -= attackPoint;
        if (Hp == 0)
        {
            GameManager.Instance.ReroadScene();
        }
    }

    /// <param name="healPoint">回復量</param>
    void Heal(int healPoint)
    {
        Hp += healPoint;
    }

    /// <summary>
    /// 左端と右端に下向きのRayを飛ばし、片方でも地面に当たればtrueを返す
    /// </summary>
    /// <returns>地面についているか</returns>
    bool IsGrounded()
    {
        RaycastHit2D right = Physics2D.Raycast(transform.position + new Vector3(_width, 0, 0), Vector3.down, _rayLength, _groundLayer);
        RaycastHit2D left = Physics2D.Raycast(transform.position + new Vector3(-_width, 0, 0), Vector3.down, _rayLength, _groundLayer);
        
        return right || left;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy" && !_isInvincible)
        {
            _isUsingGravity = false;
            _rigidbody2D.isKinematic = false;
            Damage(other.gameObject.GetComponent<Enemy>().AttackPoint);
            KnockBack(other.transform.position);
            StartCoroutine(Stun());
            StartCoroutine(BecomesInvincible());
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
        _isInvincible = true;
        gameObject.layer = LayerMask.NameToLayer(InvincibleLayer);
        yield return new WaitForSeconds(_invincibleTime);
        gameObject.layer = LayerMask.NameToLayer(PlayerLayer);
        _isInvincible = false;
    }
}
