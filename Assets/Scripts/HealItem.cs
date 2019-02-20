using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : MonoBehaviour
{
    [SerializeField] int _healPoint;

    public int HealPoint 
    {
        get
        {
            return _healPoint;
        }
    }
}
