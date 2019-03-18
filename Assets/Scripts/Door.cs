using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField, Tooltip("対応するボタン")] ButtonObject _button;

    [SerializeField, Tooltip("動く速度")] float _moveSpeed;

    // 初期位置
    Vector3 _defaultPosition;
    [SerializeField, Tooltip("目標地点(相対)")] Vector3 _targetPosition;

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
        if (_button.IsPressed)
        {
            ElapsedTime += Time.deltaTime * _moveSpeed;
        }
        else
        {
            ElapsedTime -= Time.deltaTime * _moveSpeed;
        }

        // 経過時間に合わせて移動
        transform.position = Vector3.Lerp(_defaultPosition, _defaultPosition + _targetPosition, ElapsedTime);
    }
}
