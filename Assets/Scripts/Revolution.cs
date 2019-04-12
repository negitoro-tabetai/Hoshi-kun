using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolution : BaseRevolution
{
    const string _groundLayer = "Ground";
    const int _throwCountLimit = 2;
    int _throwCount = 0;



    void OnTriggerEnter2D(Collider2D col)
    {
        if (On)
        {
            _throwCount++;
            if (_throwCount >= _throwCountLimit)
            {
                Instantiate(_revolutionBreakEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log(_throwCount);
                gameObject.GetComponentInChildren<ParticleSystem>().Stop();
                gameObject.GetComponent<Collider2D>().isTrigger = false;
                gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                gameObject.layer = LayerMask.NameToLayer(_groundLayer);
                On = false;
            }

        }
    }
}