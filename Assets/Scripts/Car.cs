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
    GravitySource _gravitySource;
    [SerializeField] Direction _direction;
    [SerializeField] float _pullSpeed;
    [SerializeField] float _speed;
    [SerializeField] float _velocity;
    [SerializeField] float _attackThreshold;
    [SerializeField, Tooltip("壁レイヤー")] LayerMask _wallLayer;
    [SerializeField, Tooltip("進行方向のフィルター")] ContactFilter2D _filter;

    MoveGround _moveGround;
    Collider2D _collider;

    bool _isPulling;
    bool _isHitFront;
    float _power;
    Vector3 _previousPosition;

    // フィルターの上向き
    const float UpNormalAngle = 270;
    // フィルターの範囲(±)
    const float AngleRange = 5;

    void Start()
    {
        _collider = GetComponent<Collider2D>();
        if (_direction == Direction.Right)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            _filter.useOutsideNormalAngle = false;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            _filter.useOutsideNormalAngle = true;
        }
        _filter.maxNormalAngle = Mathf.Repeat(180 + transform.eulerAngles.y + AngleRange, 360);
        _filter.minNormalAngle = Mathf.Repeat(180 + transform.eulerAngles.y - AngleRange, 360);
        _moveGround = GetComponent<MoveGround>();
    }

    void Update()
    {
        if (_gravitySource)
        {
            if (_isPulling && !_moveGround.IsTouching)
            {
                if ((_direction == Direction.Right && _gravitySource.transform.position.x - transform.position.x < 0) ||
                    (_direction == Direction.Left && _gravitySource.transform.position.x - transform.position.x > 0))
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

        if (_power != 0 && !_isPulling && !_isHitFront)
        {
            _velocity += _power * Time.deltaTime * _speed;
            Move();
            _velocity *= 0.99f;
            _power = Mathf.SmoothStep(_power, 0, Time.deltaTime);
        }

        if (Physics2D.IsTouching(_collider, _filter))
        {
            _isHitFront = true;
            _velocity = 0;
        }
        else
        {
            _isHitFront = false;
        }
        _previousPosition = transform.position;
    }
    void PullMove(float speed)
    {
        float posX = Mathf.Clamp((Vector3.MoveTowards(transform.position, _gravitySource.transform.position, speed * Time.deltaTime).x), WallCheck(Vector2.left), WallCheck(Vector2.right));
        transform.position = new Vector3(posX, transform.position.y);
    }

    void Move()
    {
        float posX = Mathf.Clamp((transform.position.x - _velocity * Time.deltaTime), WallCheck(Vector2.left), WallCheck(Vector2.right));
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
            if (!_gravitySource)
            {
                _gravitySource = other.transform.root.GetComponentInChildren<GravitySource>();
            }

            if (_gravitySource.IsUsingGravity)
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
        if (other.gameObject.tag == "Enemy" && Mathf.Abs(_velocity) > _attackThreshold)
        {
            other.gameObject.GetComponent<BaseEnemy>().destroy();
        }
    }
}
