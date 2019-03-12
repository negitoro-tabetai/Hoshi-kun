using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spine : MonoBehaviour
{
    [SerializeField, Tooltip("ダメージ量")] int _attackPoint;
    //"方向指定用フィルター"
    ContactFilter2D _filter;
    Collider2D _collider;

    // 前フレームの回転
    float _previousAngle;

    // フィルターの上向き
    const float UpNormalAngle = 270;
    // フィルターの範囲(±)
    const float AngleRange = 5;
    // 左向き
    const float LeftAngle = 90;

    // 角度が変わったか
    bool HasAngleChanged
    {
        get
        {
            return _previousAngle != transform.eulerAngles.z;
        }
    }

    void Start()
    {
        _collider = GetComponent<Collider2D>();
        _filter.useNormalAngle = true;
        UpdateNormalAngle();
    }
    void Update()
    {
        if (HasAngleChanged)
        {
            UpdateNormalAngle();
        }
        _previousAngle = transform.eulerAngles.z; 
    }
    
    /// <summary>
    /// フィルターの角度を自身の回転に合わせる。
    /// </summary>
    void UpdateNormalAngle()
    {
        _filter.maxNormalAngle = Mathf.Repeat(UpNormalAngle + AngleRange + transform.rotation.eulerAngles.z, 360);
        _filter.minNormalAngle = Mathf.Repeat(UpNormalAngle - AngleRange + transform.rotation.eulerAngles.z, 360);

        //左向きなら外側の範囲を使用
        if (LeftAngle - AngleRange < transform.rotation.eulerAngles.z && transform.rotation.eulerAngles.z < LeftAngle + AngleRange)
        {
            _filter.useOutsideNormalAngle = true;
        }
        else
        {
            _filter.useOutsideNormalAngle = false;
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        // 棘の先の方向からプレイヤーに当たったらプレイヤーにダメージを与える。
        if (other.gameObject.tag == "Player" && _collider.IsTouching(_filter))
        {
            other.gameObject.GetComponent<Player>().Damage(_attackPoint, transform.position);
        }
    }
}
