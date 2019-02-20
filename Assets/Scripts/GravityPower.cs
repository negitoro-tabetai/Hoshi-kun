using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPower : MonoBehaviour
{
    [SerializeField]
    private GameObject field;

    private Rigidbody rb;
    void Start()
    {
        field.SetActive(false);

        rb = GetComponent<Rigidbody>();


    }


    void Update()

    {
        //地面ついてないと使えない
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        RaycastHit hit;
        //フィールドのON,OFF
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 1.0f, layerMask) && Input.GetKeyDown(KeyCode.V))
        {
            field.SetActive(true);

            rb.isKinematic = true;

        }



        if (Input.GetKeyUp(KeyCode.V))
        {
            field.SetActive(false);
            GetComponent<Player>().enabled = true;
            rb.isKinematic = false;
        }
    }
}


