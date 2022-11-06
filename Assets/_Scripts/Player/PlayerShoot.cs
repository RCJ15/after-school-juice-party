using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The player shoot script
/// </summary> - Ruben
public class PlayerShoot : MonoBehaviour
{
    [SerializeField] protected float shootDelay;
    protected float _shootTimer;

    [SerializeField] protected Transform spawnPoint;

    [Header("Juice")]
    [SerializeField] protected ParticleSystem shootEffect;
    [SerializeField] protected bool animatePlayer = true;
    [SerializeField] protected float shakeIntensity = 0.1f;
    [SerializeField] protected float shakeDuration = 0.05f;

    protected Animator _playerAnim;

    protected bool ShootKeyDown => Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space);
    protected bool ShootKey => Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space);
    protected bool ShootKeyUp => Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.Space);

    protected virtual void Start()
    {
        _playerAnim = PlayerMove.Instance.GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        if (_shootTimer > 0)
        {
            _shootTimer -= Time.deltaTime;
            return;
        }

        // Shoot
        if (ShootKey)
        {
            Shoot();
        }
    }

    protected virtual void Shoot()
    {
        if (shootEffect != null)
        {
            shootEffect.Play();
        }

        if (animatePlayer)
        {
            DoPlayerShootAnim();
        }

        if (shakeIntensity > 0 && shakeDuration > 0)
        {
            CameraEffects.Shake(shakeIntensity, shakeDuration);
        }

        _shootTimer = shootDelay;
    }

    protected virtual void DoPlayerShootAnim()
    {
        _playerAnim.SetTrigger("Shoot");
    }
}
