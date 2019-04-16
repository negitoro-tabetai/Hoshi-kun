using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveAttraction : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] GameObject AttractionFieldCenter;
    [SerializeField, Tooltip("壁レイヤー")] LayerMask _wallLayer;
    private Rigidbody2D rigidbody;
    float _speed = 0f;
    float ACspeed = 9.81f;
    Player player;
    bool Moving = false;

    [SerializeField] GameObject field;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
       
    }
    //埋まらないようにする
    float WallCheck(Vector2 direction)
    {
        const float MaxDistance = 100;
        const float size = 0.9f;

        // RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, MaxDistance, _wallLayer);
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, transform.localScale * size, transform.eulerAngles.z, direction, MaxDistance, _wallLayer);
        if (hit)
        {
            Debug.DrawRay(transform.position, hit.point - (Vector2)transform.position, Color.red);
            return hit.point.x + Mathf.Sign(transform.position.x - hit.point.x) * Mathf.Abs((transform.rotation * transform.localScale).x) / 2;
        }
        Debug.DrawRay(transform.position, direction * MaxDistance, Color.red);
        return transform.position.x + direction.x * MaxDistance;
    }
    float WallCheckY(Vector2 direction)
    {
        const float MaxDistance = 100;
        const float size = 0.9f;

        // RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, MaxDistance, _wallLayer);
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, transform.localScale * size, transform.eulerAngles.z, direction, MaxDistance, _wallLayer);
        if (hit)
        {
            Debug.DrawRay(transform.position, hit.point - (Vector2)transform.position, Color.red);
            return hit.point.y + Mathf.Sign(transform.position.y - hit.point.y) * Mathf.Abs((transform.rotation * transform.localScale).y) / 2;
        }
        Debug.DrawRay(transform.position, direction * MaxDistance, Color.red);
        return transform.position.y + direction.y * MaxDistance;
    }

    void Update()
    {
        Debug.Log(_speed);
        //引き寄せられてるか
        if (Moving)
        {
           
            Move();
            rigidbody.gravityScale = 0f;

            if (_speed <= 12f)
            {
                _speed += (ACspeed / 2) * Time.deltaTime;
            }
                
            
        }
        else
        {
            _speed = 0f;
          
            rigidbody.gravityScale = 1f;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
          field.SetActive(false);
            Moving = false;
        }
    }
    //引き寄せられてるとき
    void Move()
    {
        //var _direction = AttractionField.transform.position - _player.transform.position;
        //_direction.Normalize();
        //rigidbody.AddForce(_speed * _direction, ForceMode2D.Force);

        float posX = Mathf.Clamp((Vector3.MoveTowards(transform.position, AttractionFieldCenter.transform.position, _speed * Time.deltaTime).x),
            WallCheck(Vector2.left), WallCheck(Vector2.right));

        float posY = Mathf.Clamp((Vector3.MoveTowards(transform.position, AttractionFieldCenter.transform.position, _speed * Time.deltaTime).y),
            WallCheckY(Vector2.down), WallCheckY(Vector2.up));
        _player.transform.position = new Vector3(posX, posY);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Afield")
        {
            Moving = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Afield")
        {
            Moving = false;
        }
    }
}
