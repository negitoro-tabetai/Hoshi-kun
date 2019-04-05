using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolution : BaseRevolution
{
    const string _groundLayer = "Ground";


    void OnTriggerEnter2D(Collider2D col)
    {
        if (On)
        {
            gameObject.GetComponentInChildren<ParticleSystem>().Stop();
            gameObject.GetComponent<Collider2D>().isTrigger = false;
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            gameObject.layer = LayerMask.NameToLayer(_groundLayer);
        }
        if(On && col.gameObject.tag == "Enemy")
        {
            Instantiate(_revolutionBreakEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}