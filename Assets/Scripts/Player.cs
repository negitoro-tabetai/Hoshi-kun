using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody _rigidbody;

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

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jump();
        }

        // LeftShiftキーで走る
        if (Input.GetButton("Fire3"))
        {
            _isRunning = true;
        }
        else
        {
            _isRunning = false;
        }

        _inputX = Input.GetAxisRaw("Horizontal");
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
        _rigidbody.AddForce((Vector3.up + Vector3.right) * _jumpForce, ForceMode.Impulse);
    }

    void KnockBack(Vector3 enemyPosition)
    {
        _rigidbody.velocity = Vector3.zero;
        const float HorizontalForce = 2;
        
        Vector3 direction = new Vector3 (transform.position.x - enemyPosition.x, 0, 0).normalized;
        direction = new Vector3 (direction.x, HorizontalForce, 0).normalized;
        _rigidbody.AddForce(direction * _knockBackForce, ForceMode.Impulse);
    }

    /// <summary>
    /// ダメージをくらう
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    void Damage(int damage)
    {
        Hp -= damage;
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