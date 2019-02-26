using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    [SerializeField]
    GameObject StartObject;
    [SerializeField]
    GameObject EndObject;
    [SerializeField]
    GameObject Player;
    private float elapsedTime;
    private Vector3 StartPosition;
    private Vector3 EndPosition;
    private bool Moves = false;
    private Rigidbody2D _rigidbody;
    [SerializeField] private float _speed;
    private bool _IsUsingGravity = false;
    void Start()
    {

        StartPosition = StartObject.transform.position;
        EndPosition = EndObject.transform.position;
        this.transform.position = StartPosition;
        _rigidbody = GetComponent<Rigidbody2D>();

    }
    void Update()
    {
        if (Input.GetKey(KeyCode.V))
        {
            Moves = true;


        }
        if (Input.GetKeyUp(KeyCode.V))
        {
            Moves = false;
        }


    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "field")
        {
            Moves = true;
        }
    }



    void FixedUpdate()
    {
        //オブジェクトの移動
        float step = _speed * Time.fixedDeltaTime;
        if (_IsUsingGravity && Moves)
        {
            // _rigidbody.MovePosition(transform.position = Vector3.MoveTowards(transform.position, EndPosition, step));
            Vector3 targetPosition = Vector3.Lerp(StartPosition, EndPosition, (StartPosition.y - Player.transform.position.y) / (StartPosition.y - EndPosition.y));
            _rigidbody.MovePosition(Vector3.MoveTowards(transform.position, targetPosition, step));
        }
        else
        {
            _rigidbody.MovePosition(Vector3.MoveTowards(transform.position, StartPosition, step));
        }

    }
}
