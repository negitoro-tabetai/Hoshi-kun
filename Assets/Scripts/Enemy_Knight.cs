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
    [SerializeField] float k_speed;
    [SerializeField] string _myArmor = "armor";
    RaycastHit2D find;
    Animator _animator;
    bool _damage;
    //----------------------------------------------------------


    new void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
    }


    void Update()
    { 
        if (!_damage)
        {
            find = Physics2D.BoxCast(transform.position, transform.localScale, 0, transform.right, _findRayLength, _playerLayer);
            if (find && CanMoveForward())
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
        _rigidbody.velocity = new Vector2(transform.right.x * k_speed, _rigidbody.velocity.y);
    }


    void DamageMove()
    {
        _speed = 4f;
        _moveTime = 0.5f;
        Move();
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "RevolutionBlock")
        {
            _damage = true;
            _animator.SetTrigger("_damage");
        }    
    }


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
