using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableBlock : MonoBehaviour
{
    //プレイヤー
    Player _player;
    float _speed = 5;
    //移動スピード
    [SerializeField, Tooltip("壁レイヤー")] LayerMask _wallLayer;

    bool _isMoving;

    void Update()
    {
        if (_player)
        {
            if (_isMoving)
            {
                Move();
            }
        }
    }

    void Move()
    {
        float posX = Mathf.Clamp((Vector3.MoveTowards(transform.position, _player.transform.position - (_player.transform.position - transform.position).normalized, _speed * Time.deltaTime).x), WallCheck(Vector2.left), WallCheck(Vector2.right));
        transform.position = new Vector3(posX, transform.position.y);
    }

    float WallCheck(Vector2 direction)
    {
        const float MaxDistance = 100;
        const float size = 0.9f;

        // RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, MaxDistance, _wallLayer);
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, transform.localScale * size, transform.eulerAngles.z, direction, MaxDistance, _wallLayer);
        if (hit)
        {
            Debug.DrawRay(transform.position, hit.point - (Vector2)transform.position, Color.red);
            return hit.point.x + Mathf.Sign(transform.position.x - hit.point.x) * Mathf.Abs((transform.rotation * transform.localScale).x) / 2;
        }
        Debug.DrawRay(transform.position, direction * MaxDistance, Color.red);
        return transform.position.x + direction.x * MaxDistance;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Field")
        {
            if (!_player)
            {
                _player = other.transform.root.GetComponent<Player>();
            }

            if (_player.IsUsingGravity)
            {
                _isMoving = true;
            }
            else
            {
                _isMoving = false;
            }
        }
    }
}
