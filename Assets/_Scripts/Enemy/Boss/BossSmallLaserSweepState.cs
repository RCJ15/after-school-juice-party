using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class BossSmallLaserSweepState : BossState
{
    [SerializeField] private float screenShakeIntensity;
    [SerializeField] private float screenShakeDuration;
    [SerializeField] private float flashDuration;
    [SerializeField] private Color flashColor;

    [Header("Transforms")]
    [SerializeField] private Transform laserIndicator;
    [SerializeField] private Transform checkPosTarget;
    [SerializeField] private Transform checkPosStart;
    [SerializeField] private LineRenderer laser;
    private Transform _laserTransform;
    private Transform _parent;
    private Vector3 _startSize;

    [Space]
    [SerializeField] private Vector2 rngRange = new Vector2(-60, 60);
    private float _currentRot;
    [SerializeField] private float[] size;
    [SerializeField] private float laserMaxWidth = 0.3f;
    private float _currentSize;

    private float[] _startAngles;
    private float[] _targetAngles;
    private Quaternion _currentStartRot;
    private Quaternion _currentTargetRot;

    [Header("Anim Values")]
    [SerializeField] private float rotationAmount;
    [SerializeField] private float indicatorSize;
    [SerializeField] private float laserWidth;
    [SerializeField] private float laserSweepAmount;

    private void Awake()
    {
        _startSize = laserIndicator.localScale;
        _laserTransform = laser.transform;

        int length = size.Length;
        _startAngles = new float[length];
        _targetAngles = new float[length];

        for (int i = 0; i < length; i++)
        {
            SetSize(size[i]);

            _targetAngles[i] = Vector2.SignedAngle(transform.position, checkPosTarget.position);
            _startAngles[i] = Vector2.SignedAngle(transform.position, checkPosStart.position);
        }

        _parent = transform.parent;
    }

    private void OnEnable()
    {
        Animate("Small Laser Sweep");

        _currentRot = Random.Range(rngRange.x, rngRange.y);
        _currentSize = size[Stage - 1];
        _currentTargetRot = Quaternion.Euler(0, 0, _targetAngles[Stage - 1]);
        _currentStartRot = Quaternion.Euler(0, 0, _startAngles[Stage - 1]);
    }

    protected override void Update()
    {
        base.Update();

        SetSize(indicatorSize * _currentSize);
        laser.widthMultiplier = laserWidth * laserMaxWidth;
        _laserTransform.localScale = new Vector3(laserWidth, 1, 1);
        _laserTransform.localRotation = Quaternion.Slerp(_currentTargetRot, _currentStartRot, laserSweepAmount);
        _parent.transform.rotation = Quaternion.Euler(0, 0, rotationAmount * _currentRot);
    }

    private void SetSize(float size)
    {
        laserIndicator.localScale = new Vector3(size, _startSize.y, 1);
    }

    public void SmallLaserSweep()
    {
        CameraEffects.Shake(screenShakeIntensity, screenShakeDuration);
        CameraEffects.Flash(flashDuration, flashColor);
    }
}
