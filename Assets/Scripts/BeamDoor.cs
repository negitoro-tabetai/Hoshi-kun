using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BeamDoor : MonoBehaviour
{
    enum Type
    {
        // どれかひとつでも押されていれば開く
        Any,
        // 全て押されていれば開く
        All
    }
    [SerializeField, Tooltip("対応するスイッチ")] BeamSwitch[] _switch;
    [SerializeField, Tooltip("複数ある場合の開く条件")] Type _type;
    [SerializeField, Tooltip("動く速度")] float _moveSpeed;

    // 初期位置
    Vector3 _defaultPosition;
    [SerializeField, Tooltip("目標地点(相対)")] Vector3 _targetPosition;
    [SerializeField] LayerMask _mask;

    // 経過時間
    float _elapsedTime;
    float ElapsedTime
    {
        get
        {
            return _elapsedTime;
        }
        set
        {
            _elapsedTime = Mathf.Clamp01(value);
        }
    }
    void Start()
    {
        _defaultPosition = transform.position;
    }

    void Update()
    {
        bool isOpen;
        if (_type == 0)
        {
            isOpen = _switch.Any(s => s.IsOn);
        }
        else
        {
            isOpen = _switch.All(s => s.IsOn);
        }

        if (isOpen)
        {
            ElapsedTime += Time.deltaTime * _moveSpeed;
        }
        else
        {
            // 間にものがあった場合
            ElapsedTime = Mathf.Clamp(ElapsedTime - Time.deltaTime * _moveSpeed, 1 - (GetHitPosition() - (_defaultPosition + _targetPosition)).magnitude / _targetPosition.magnitude, 1);
        }
        // 経過時間に合わせて移動
        transform.position = Vector3.Lerp(_defaultPosition, _defaultPosition + _targetPosition, ElapsedTime);

    }
    Vector3 GetHitPosition()
    {
        const float MaxDistance = 100;
        const float size = 0.9f;

        RaycastHit2D hit = Physics2D.BoxCast(_defaultPosition + _targetPosition, transform.localScale * size, transform.eulerAngles.z, -_targetPosition, MaxDistance, _mask);
        if (hit)
        {
            Debug.DrawRay(transform.position, hit.point - (Vector2)transform.position, Color.blue);
            return new Vector3(transform.position.x, hit.point.y + Mathf.Sign(transform.position.y - hit.point.y) * Mathf.Abs((transform.rotation * transform.localScale).y) / 2, transform.position.z);
        }
        return _targetPosition;
    }
}
