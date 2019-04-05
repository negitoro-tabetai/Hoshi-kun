using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRevolution : BaseRevolution
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (On)
        {
            Instantiate(_revolutionBreakEffect, transform.position, Quaternion.identity);
            gameObject.GetComponentInChildren<ParticleSystem>().Stop();
            Destroy(gameObject);
        }
    }
}
