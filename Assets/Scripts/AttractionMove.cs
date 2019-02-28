using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractionMove : MonoBehaviour
{
    [SerializeField, Tooltip("引き寄せられる速さ")] float _speed;
    [SerializeField] GrantBlock _grantBlock;
    Vector3 _convertionPosition;
    Rigidbody2D _rigidbody;
    [SerializeField] ContactFilter2D _filter;
    [SerializeField] GameObject _player;
    bool _isTouch = false;


    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.isKinematic = true;
        _filter.SetNormalAngle(250, 290);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    /// <summary>
    /// 付与ブロックのスイッチがONになった時付与ブロックに向かって進む
    /// </summary>
    void Move()
    {
        if (_grantBlock.IsOn && !_isTouch)
        {
            transform.position = Vector3.MoveTowards(transform.position, _grantBlock.gameObject.transform.position , _speed * Time.deltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "GrantBlock")
        {
            _isTouch = true;
        }

        if(collision.gameObject.tag == "Player" && _rigidbody.IsTouching(_filter))
        {
            _player.transform.SetParent(transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            _player.transform.SetParent(null);
        }
    }

}
