using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    enum Direction
    {
        Right,
        Left
    }
    Player _player;
    [SerializeField] Direction _direction;
    [SerializeField] float _pullSpeed;
    [SerializeField] float _speed;
    [SerializeField] float _velocity = 0;
    [SerializeField, Tooltip("壁レイヤー")] LayerMask _wallLayer;

    bool _isPulling;
    bool _isMoving;
    bool _isTouching;
    float _power;
    Vector3 _previousPosition;
    void Update()
    {

        if (_player)
        {
            if (_isPulling && !_isTouching)
            {
                if ((_direction == Direction.Right && _player.transform.position.x - transform.position.x < 0) ||
                    (_direction == Direction.Left && _player.transform.position.x - transform.position.x > 0))
                {
                    PullMove(_pullSpeed);
                    float moveX = (transform.position.x - _previousPosition.x);
                    _power += moveX;
                }
                else
                {
                    PullMove(_speed);
                }
            }
        }

        if (_power != 0 && !_isPulling)
        {
            _velocity += _power * Time.deltaTime * 10;
            Move();
            _velocity *= 0.99f;
            _power = Mathf.SmoothStep(_power, 0, Time.deltaTime * 10);
        }
        _previousPosition = transform.position;
    }
    void PullMove(float speed)
    {
        float posX = Mathf.Clamp((Vector3.MoveTowards(transform.position, _player.transform.position, speed * Time.deltaTime).x), WallCheck(Vector2.left), WallCheck(Vector2.right));
        transform.position = new Vector3(posX, transform.position.y);
    }

    void Move()
    {
        float posX = Mathf.Clamp((Vector3.MoveTowards(transform.position, transform.position + transform.right * (_direction == 0 ? -1 : 1), _velocity * Time.deltaTime).x), WallCheck(Vector2.left), WallCheck(Vector2.right));
        transform.position = new Vector3(posX, transform.position.y);
    }

    float WallCheck(Vector2 direction)
    {
        const float MaxDistance = 100;
        const float size = 0.9f;

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
                _isPulling = true;
            }
            else
            {
                _isPulling = false;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Field")
        {
            _isPulling = false;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _isTouching = true;
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _isTouching = false;
        }
    }
}
