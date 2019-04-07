using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour
{
    [SerializeField, Tooltip("始点")] Transform _start;
    [SerializeField, Tooltip("終点")] Transform _end;
    LineRenderer _lineRenderer;


    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.startWidth = 0.1f;
        _lineRenderer.endWidth = 0.1f;
        //_lineRenderer.useWorldSpace = true;

    }

    // Update is called once per frame
    void Update()
    {
        _lineRenderer.SetPosition(0, _start.position);
        _lineRenderer.SetPosition(1, _end.position);
    }
}
