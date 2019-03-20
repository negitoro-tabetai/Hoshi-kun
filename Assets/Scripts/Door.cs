using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Door : MonoBehaviour
{
    enum ButtonType
    {
        // どれかひとつでも押されていれば開く
        Any,
        // 全て押されていれば開く
        All
    }
    [SerializeField, Tooltip("対応するボタン")] ButtonObject[] _buttons;
    [SerializeField, Tooltip("複数ボタンがある場合の開く条件")] ButtonType _buttonType;
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
        bool isOpen;
        if (_buttonType == 0)
        {
            isOpen = _buttons.Any(button => button.IsPressed);
        }
        else
        {
            isOpen = _buttons.All(button => button.IsPressed);
        }
        
        if (isOpen)
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
