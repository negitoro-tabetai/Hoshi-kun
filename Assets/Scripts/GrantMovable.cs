using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrantMovable : MonoBehaviour
{
    //---------------------------------------------------------------
    //変数宣言
    [SerializeField, Tooltip("引き寄せられる速さ")] float _speed;
    [SerializeField] GrantBlock _grantBlock;
    [SerializeField] LayerMask _wallLayer;
    //---------------------------------------------------------------


    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    /// <summary>
    /// 付与ブロックのスイッチがONになった時付与ブロックに向かって進む
    /// </summary>
    void Move()
    {
        //付与ブロックがONで且つ付与ブロックに触れていないとき
        if (_grantBlock.IsOn)
        {
            Vector3 position = Vector3.MoveTowards(transform.position, _grantBlock.transform.position, _speed * Time.deltaTime);
            position.x = Mathf.Clamp(position.x, WallCheck(Vector2.left), WallCheck(Vector2.right));
            position.y = Mathf.Clamp(position.y, WallCheck(Vector2.down), WallCheck(Vector2.up));
            transform.position = position;
        }
    }

    float WallCheck(Vector2 direction)
    {
        const float MaxDistance = 100;
        const float size = 0.9f;

        RaycastHit2D hit = Physics2D.BoxCast(transform.position, transform.localScale * size, transform.eulerAngles.z, direction, MaxDistance, _wallLayer);
        if (hit)
        {
            Debug.DrawRay(transform.position, hit.point - (Vector2)transform.position, Color.red);
            if (direction.x != 0)
            {
                return hit.point.x + Mathf.Sign(transform.position.x - hit.point.x) * Mathf.Abs((transform.rotation * transform.localScale).x) / 2;
            }
            else
            {
                return hit.point.y + Mathf.Sign(transform.position.y - hit.point.y) * Mathf.Abs((transform.rotation * transform.localScale).y) / 2;
            }
        }
        Debug.DrawRay(transform.position, direction * MaxDistance, Color.red);
        if (direction.x != 0)
        {
            return transform.position.x + direction.x * MaxDistance;
        }
        else
        {
            return transform.position.x + direction.y * MaxDistance;
        }
    }
}
