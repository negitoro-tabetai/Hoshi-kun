using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D _rigidbody;
    Collider2D _collider;
    GravitySource _gravitySource;
    Animator _animator;

    [SerializeField, Tooltip("モデルのオブジェクト")] GameObject _model;
    [SerializeField, Tooltip("体力")] int _hp;
    [SerializeField, Tooltip("移動速度")] float _moveSpeed;
    [SerializeField, Tooltip("無敵時間")] float _invincibleTime;
    [SerializeField, Tooltip("硬直時間")] float _stunningTime;
    [SerializeField, Tooltip("ジャンプの強さ")] float _jumpForce;
    [SerializeField, Tooltip("ノックバックの強さ")] float _knockBackForce;
    [SerializeField, Tooltip("判定用フィルター")] ContactFilter2D _upFilter, _downFilter;
    // 左右入力
    float _inputX;
    // 走っているか
    bool _isMoving;
    // 硬直しているか
    bool _isStunning;

    Vector3 _previousPosition;

    // Layer名
    const string PlayerLayer = "Player";
    const string InvincibleLayer = "InvinciblePlayer";

    const int MaxHP = 5;

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
        _gravitySource = GetComponent<GravitySource>();
        _animator = GetComponentInChildren<Animator>();

        // リスポーン地点から
        if (GameManager.Instance.RespawnPoint != Vector3.zero)
        {
            transform.position = GameManager.Instance.RespawnPoint;
        }
    }

    void Update()
    {
        if (!GameManager.Instance.IsPausing)
        {

            // Vキーで引力使用
            if (Input.GetButtonDown("UseGravity"))
            {
                _animator.SetBool("Gravity", true);
                _gravitySource.IsUsingGravity = true;
            }
            if (Input.GetButtonUp("UseGravity"))
            {
                _animator.SetBool("Gravity", false);
                _gravitySource.IsUsingGravity = false;
            }

            if (Input.GetButtonDown("Jump") && Physics2D.IsTouching(_collider, _downFilter) && !Physics2D.IsTouching(_collider, _upFilter) && !_isStunning)
            {
                Jump();
            }

            // 左右入力
            _inputX = System.Math.Sign(Input.GetAxisRaw("Horizontal"));
            if (_inputX > 0)
            {
                _model.transform.localScale = new Vector3(1, 1, 1);
            }
            if (_inputX < 0)
            {
                _model.transform.localScale = new Vector3(-1, 1, 1);
            }

            _isMoving = Mathf.Abs(_previousPosition.x - transform.position.x) > Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        if (!_isStunning)
        {
            _rigidbody.velocity = new Vector3(_inputX * _moveSpeed, _rigidbody.velocity.y, 0);
        }
        _previousPosition = transform.position;
    }

    void Jump()
    {
        _animator.SetTrigger("Jump");
        _rigidbody.AddForce((Vector2.up) * _jumpForce, ForceMode2D.Impulse);
    }

    /// <param name="attackPoint">ダメージ量</param>
    /// <param name="enemyPosition">衝突した敵の位置</param>
    public void Damage(int attackPoint, Vector3 enemyPosition)
    {
        if (!IsInvincible)
        {
            _animator.SetTrigger("Damage");
            _gravitySource.IsUsingGravity = false;
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
