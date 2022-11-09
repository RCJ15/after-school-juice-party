using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class BossIdleState : BossState
{
    [Header("Sine wave bobbing")]
    [SerializeField] private float sineWaveSpeed = 0.3f;
    [SerializeField] private float sineWaveIntensity = 0.75f;

    private float _sineTime;
    private Vector3 _startPos;

    protected override void Start()
    {
        base.Start();

        _startPos = transform.localPosition;
    }

    protected override void Update()
    {
        base.Update();

        // Sine wave bobbing - Ruben
        float yPos = _startPos.y + Mathf.Sin(_sineTime) * sineWaveIntensity;

        _sineTime += Time.deltaTime * sineWaveSpeed;

        transform.localPosition = new Vector3(transform.localPosition.x, yPos);
    }
}
