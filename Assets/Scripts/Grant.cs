using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grant : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Collider2D>().isTrigger = true;
        GetComponent<GravitySource>().IsUsingGravity = true;
    }
}
