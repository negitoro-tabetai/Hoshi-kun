using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(MovableBlock))]
public class Enemy_Knight : BaseEnemy
{
    //----------------------------------------------------------
    //変数宣言
    [SerializeField] float _findRayLength;
    [SerializeField] LayerMask _playerLayer;
    [SerializeField, Tooltip("プレイヤーに追尾する速度")] float followSpeed;
    [SerializeField, Tooltip("knightの鎧の名前")] string _myArmor = "armor";
    [SerializeField, Tooltip("ダメージ後の方向転換の速度")] float _afterMoveTime;
    [SerializeField, Tooltip("ダメージ後の動きの速さ")] float _afterSpeed;
    Animator _animator;
    bool _damage;
    //----------------------------------------------------------

    /// <summary>
    /// 向いている方向にプレイヤーがいるか確認する関数
    /// </summary>
    /// <returns>向いてる方向にBoxcastを打った結果</returns>
    bool Find()
    {
        return Physics2D.BoxCast(transform.position, transform.localScale, 0, transform.right, _findRayLength, _playerLayer);
    }
   

    new void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
    }


    void Update()
    { 
        if (!_damage)
        {
            if (Find() && CanMoveForward())
            //向いている方向にプレイヤーがいた場合、且つ、地面と接している場合
            {
                LookAtPlayer();
                Follow();
            }
            else
            {
                Move();
            }
        }
        else
        {
            DamageMove();
        }
    }


    // Update is called once per frame
    void Follow()
    {
        //float moveCheck = find.point.x + Mathf.Sign(transform.position.x - find.point.x) * transform.localScale.x;
        //Debug.Log(moveCheck);
        //Debug.Log("ナイトの移動です");
        _rigidbody.velocity = new Vector2(transform.right.x * followSpeed, _rigidbody.velocity.y);
    }


    void DamageMove()
    {
        Move(_afterSpeed, _afterMoveTime);
    }


    new void OnTriggerEnter2D(Collider2D collider)
    {
        base.OnTriggerEnter2D(collider);
        if (collider.gameObject.tag == "RevolutionBlock")
        {
            _damage = true;
            _animator.SetTrigger("_damage");
        }
    }


    /// <summary>
    /// Knightの子オブジェクトに鎧があった場合消す関数
    /// </summary>
    void ArmorBreak()
    {
        foreach(Transform childTransform in transform)
        {
            if(childTransform.name == _myArmor)
            {
                Destroy(childTransform.gameObject);
            }
        }
    }
}
