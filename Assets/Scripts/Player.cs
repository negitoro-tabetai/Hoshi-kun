using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody _rigidbody;
    SphereCollider _sphereCollider;

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

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded() && !_isStunning && !_isUsingGravity)
        {
            Jump();
        }

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
                Debug.Log("UseGravity");
            }
            if (Input.GetButtonUp("UseGravity"))
            {
                _isUsingGravity = false;
            }
        }

        // 引力を使用していないときは左右入力を受け付ける
        if (!_isUsingGravity)
        {
            _inputX = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            _inputX = 0;
        }
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
    }

    void Jump()
    {
        _rigidbody.AddForce((Vector3.up) * _jumpForce, ForceMode.Impulse);
    }

    /// <param name="enemyPosition">衝突した敵の位置</param>
    void KnockBack(Vector3 enemyPosition)
    {
        _rigidbody.velocity = Vector3.zero;
        const float VerticalForce = 2;

        //自分の位置と敵の位置を比較して方向を決定
        Vector3 direction = new Vector3(transform.position.x - enemyPosition.x, 0, 0).normalized;
        direction = new Vector3(direction.x, VerticalForce, 0).normalized;
        _rigidbody.AddForce(direction * _knockBackForce, ForceMode.Impulse);
    }

    /// <param name="attackPoint">ダメージ量</param>
    void Damage(int attackPoint)
    {
        Hp -= attackPoint;
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
        Ray rayRight = new Ray(transform.position + new Vector3(_width, 0, 0), Vector3.down);
        Ray rayLeft = new Ray(transform.position + new Vector3(-_width, 0, 0), Vector3.down);

        bool isGrounded = Physics.Raycast(rayLeft, _rayLength, _groundLayer) || Physics.Raycast(rayRight, _rayLength, _groundLayer);

        return isGrounded;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy" && !_isInvincible)
        {
            Damage(other.gameObject.GetComponent<Enemy>().AttackPoint);
            KnockBack(other.transform.position);
            StartCoroutine(Stun());
            StartCoroutine(BecomesInvincible());
        }
    }

    void OnTriggerEnter(Collider other)
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