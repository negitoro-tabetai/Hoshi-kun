using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamSwitch : MonoBehaviour
{
    Collider2D _collider;
    [SerializeField, Tooltip("ビームのオブジェクト")] Beam _beam;
    public bool IsOn { get; set; } = false;

    void Start()
    {
        _collider = GetComponent<Collider2D>();
    }
    void Update()
    {
        if (_beam.HitCollider == _collider)
        {
            IsOn = false;
        }
        else
        {
            IsOn = true;
        }
    }
}
