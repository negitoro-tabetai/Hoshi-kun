using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour
{
    [SerializeField, Tooltip("始点")] Transform _pivot;
    LineRenderer _lineRenderer;

    [SerializeField, Tooltip("重さ, スピードに関係")] float _mass = 3;
    [SerializeField, Tooltip("紐の長さ")] float _length = 7;

    GravitySource _gravitySource;

    bool _isMoving;

    // 角度
    float _angle;
    // 角速度
    float _angleVelocity;
    MoveGround _moveGround;
    [SerializeField, Tooltip("減衰係数"), Range(0, 1)] float damping = 0.995f;


    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.startWidth = 0.1f;
        _lineRenderer.endWidth = 0.1f;

        _angle = 180.0f;
        _moveGround = GetComponent<MoveGround>();
    }

    void Update()
    {
        _lineRenderer.SetPosition(0, _pivot.position);
        _lineRenderer.SetPosition(1, transform.position);

        if (_isMoving && !_moveGround.IsTouching)
        {
            _angle = Mathf.LerpAngle(_angle, Mathf.Atan2(_gravitySource.transform.position.x - _pivot.position.x, _gravitySource.transform.position.y - _pivot.position.y) * Mathf.Rad2Deg, Time.deltaTime);

            _angleVelocity = 0;
        }
        _angleVelocity += -_mass * Physics2D.gravity.y * Mathf.Sin(_angle * Mathf.Deg2Rad) * Time.deltaTime;

        // 減衰
        _angleVelocity *= damping;

        _angle += _angleVelocity * Time.deltaTime;
        transform.localPosition = new Vector3(Mathf.Sin(_angle * Mathf.Deg2Rad), Mathf.Cos(_angle * Mathf.Deg2Rad)) * _length;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Field")
        {
            _gravitySource = other.transform.root.GetComponent<GravitySource>();

            if (_gravitySource.IsUsingGravity)
            {
                _isMoving = true;
            }
            else
            {
                _isMoving = false;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Field")
        {

            _isMoving = false;
        }
    }
}
