using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody _rigidbody;
    [SerializeField, Tooltip("移動してからの経過時間")] float _time = 0;
    [SerializeField, Tooltip("移動する制限時間")] float _moveTime;
    [SerializeField] float _speed;
    [SerializeField] int _life;
    const int _rotationAngle = 180;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveTimer();
        Damage();
        
    }

    void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(Vector2.right.x * _speed , _rigidbody.velocity.y);
    }
    void MoveTimer()
    {
        _time += Time.deltaTime;
        if(_time > _moveTime)
        {
            Dir();
            _time = 0;
        }
        
    }

    void Damage()
    {
        if (Input.GetKeyDown(KeyCode.B) && _life != 0)
        {
            _life--;
        }
        if (_life == 0)
        {
            Destroy(gameObject);
        }
    }

    void Dir()
    {
        Debug.Log("振り向いた");
        transform.Rotate(0, _rotationAngle, 0);
    }
}
