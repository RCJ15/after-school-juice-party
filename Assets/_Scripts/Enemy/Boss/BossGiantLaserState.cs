using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class BossGiantLaserState : BossState
{
    [SerializeField] private float screenShakeIntensity;
    [SerializeField] private float screenShakeDuration;
    [SerializeField] private float flashDuration;
    [SerializeField] private Color flashColor;

    [Header("Trails")]
    [SerializeField] private LineRenderer warning;
    private Transform _warningTransform;
    [SerializeField] private LineRenderer laser;
    private Transform _laserTransform;

    [Space]
    [SerializeField] private float[] laserSize;
    private float _currentLaserSize;

    [SerializeField] private float animatedWarningSize;
    [SerializeField] private float animatedLaserSize;

    private void Awake()
    {
        _warningTransform = warning.transform;
        _laserTransform = laser.transform;
    }

    private void OnEnable()
    {
        Animate("Giant Laser");
    }

    private void OnDisable()
    {
        Die();
    }

    protected override void Update()
    {
        base.Update();

        warning.widthMultiplier = animatedWarningSize * _currentLaserSize;
        _warningTransform.localScale = warning.widthMultiplier * Vector3.one;

        laser.widthMultiplier = animatedLaserSize * _currentLaserSize;
        _laserTransform.localScale = laser.widthMultiplier * Vector3.one;
    }

    public void ScreenShake()
    {
        CameraEffects.Shake(screenShakeIntensity, screenShakeDuration);
        CameraEffects.Flash(flashDuration, flashColor);
    }

    public override void UpdateStage(int stage)
    {
        _currentLaserSize = laserSize[stage - 1];
    }

    public override void Die()
    {
        warning.widthMultiplier = 0;
        _warningTransform.localScale = Vector3.zero;

        laser.widthMultiplier = 0;
        _laserTransform.localScale = Vector3.zero;
    }
}
