using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    [SerializeField, Tooltip("太さ")] float _width;
    [SerializeField, Tooltip("当たるレイヤー")] LayerMask _layer;

    // Lineは仮
    LineRenderer _line;

    RaycastHit2D _hit;
    const float MaxDistance = 100;

    public Collider2D HitCollider {
        get
        {
            return _hit.collider;
        }
    }

    void Start()
    {
        _line = GetComponent<LineRenderer>();
        _line.startWidth = _width;
        _line.endWidth = _width;
        _line.SetPosition(0, transform.position);
    }

    void Update()
    {
        _hit = Physics2D.CircleCast(transform.position, _width / 2, transform.up, MaxDistance, _layer);
        if (_hit)
        {
            _line.SetPosition(1, transform.position + transform.up * (_hit.distance + _width / 2));
            
            // プレイヤーに当たった場合
            if (_hit.collider.tag == "Player")
            {
                _hit.collider.GetComponent<Player>().Damage(5, transform.position);
            }
        }
        else
        {
            _line.SetPosition(1, transform.position + transform.up * MaxDistance);
        }
    }
}
