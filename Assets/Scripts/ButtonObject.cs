using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonObject : MonoBehaviour
{
    [SerializeField, Tooltip("押される速度")] float _pressSpeed;
    [SerializeField, Tooltip("ボタンのオブジェクト")] GameObject _button;
    [SerializeField, Tooltip("押されている時のボタンの大きさ")] Vector3 _pressedScale;

    Collider2D _collider;
    // 触れているコライダーを格納する配列
    Collider2D[] _contacts;

    bool _sound=true;

    [SerializeField, Tooltip("押せるオブジェクトの設定")] ContactFilter2D _filter;

    // 経過時間
    float _elapsedTime;

    /// <summary>
    /// 押されているか
    /// </summary>
    public bool IsPressed
    {
        get
        {
            return ElapsedTime == 1.0f;
        }
    }

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
        _collider = GetComponent<Collider2D>();
        _contacts = new Collider2D[1];
    }

    void Update()
    {
        if (0 < _collider.OverlapCollider(_filter, _contacts))
        {
            ElapsedTime += Time.deltaTime * _pressSpeed;
            if (_sound)
            {
                AudioManager.Instance.PlaySE("ButtonPush");
                _sound = false;
            }

        }
        else
        {
            ElapsedTime -= Time.deltaTime * _pressSpeed;
            _sound = true;
        }
        
        // 経過時間に合わせてスケールを変更
        _button.transform.localScale = Vector3.Lerp(Vector3.one, _pressedScale, ElapsedTime);
    }
}
