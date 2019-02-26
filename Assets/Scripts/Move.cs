using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField]
    GameObject StartObject;
    [SerializeField]
    GameObject EndObject;
    private float elapsedTime;
    private Vector3 StartPosition;
    private Vector3 EndPosition;
    private bool Moves = false;
    private Rigidbody2D _rigidbody;
    [SerializeField] private float _speed;
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
        if (elapsedTime >= 5)
        {

            elapsedTime = 0;
        }

    }




    void FixedUpdate()
    {
        //オブジェクトの移動
        float step = _speed * Time.fixedDeltaTime;
        if (Moves)
        {
            _rigidbody.MovePosition(transform.position = Vector3.MoveTowards(transform.position, EndPosition, step));

        }
        else
        {
            _rigidbody.MovePosition(transform.position = Vector3.MoveTowards(transform.position, StartPosition, step));
        }

    }
    void Ray()
    {
        int distance = 10;
        Ray ray = new Ray(transform.position, new Vector3(0, -1, 0));
        Debug.DrawLine(ray.origin, ray.direction * distance, Color.red);
    }
}
