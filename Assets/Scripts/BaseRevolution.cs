using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRevolution : MonoBehaviour
{
    [SerializeField]protected GameObject _revolutionBreakEffect;

    public GameObject Effect
    {
        set
        {
            _revolutionBreakEffect = value;
        }
    }

    public bool On { get; set; }
}
