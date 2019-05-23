using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Knight : BaseEnemy
{
    //----------------------------------------------------------
    //変数宣言
    [SerializeField, Tooltip("knightの鎧の名前")] string _myArmor = "armor";
    [SerializeField] float _findRayLength;
    [SerializeField, Tooltip("ダメージ後の方向転換の速度")]protected float _afterMoveTime;
    [SerializeField, Tooltip("ダメージ後の動きの速さ")]protected float _afterSpeed;
    [SerializeField, Tooltip("プレイヤーに追尾する速度")]protected float _followSpeed;
    protected bool _damage;
    [SerializeField] LayerMask _playerLayer;
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
    }


    protected void Update()
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
    protected virtual void Follow()
    {
        //Debug.Log("ナイトの移動です");
        _rigidbody.velocity = new Vector2(transform.right.x * _followSpeed, _rigidbody.velocity.y);
    }


    protected virtual void DamageMove()
    {
        Move(_afterSpeed, _afterMoveTime);
    }


    new void OnTriggerEnter2D(Collider2D collider)
    {
        base.OnTriggerEnter2D(collider);
        if (collider.gameObject.tag == "RevolutionBlock")
        {
            _damage = true;
            ArmorBreak();
        }
    }


    /// <summary>
    /// Knightの子オブジェクトに鎧があった場合消す関数
    /// </summary>
    public void ArmorBreak()
    {
        foreach(Transform childTransform in transform)
        {
            if(childTransform.name == _myArmor)
            {
                Destroy(childTransform.gameObject);
                AudioManager.Instance.PlaySE("Metal");

            }
        }


        EnemyRevolution revolution = gameObject.AddComponent<EnemyRevolution>();
        revolution.Effect = _destroyEffect;
    }
}
