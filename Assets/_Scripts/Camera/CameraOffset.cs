using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class CameraOffset : MonoBehaviour
{
    [SerializeField] private Transform offsetLooker;
    [SerializeField] private float divider = 20;
    [SerializeField] private float lerpDelta = 10;

    private Vector3 _startPos;
    private Vector3 _objStartPos;

    private void Start()
    {
        _startPos = transform.localPosition;
        _objStartPos = offsetLooker.position;
    }

    private void Update()
    {
        Vector3 target = _startPos - ((_startPos - (offsetLooker.position - _objStartPos)) / divider);
        target.z = _startPos.z;

        transform.localPosition = Vector3.Lerp(transform.localPosition, target, lerpDelta * Time.deltaTime);
    }
}
