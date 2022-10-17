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

    protected bool ShootKeyDown => Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space);
    protected bool ShootKey => Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space);
    protected bool ShootKeyUp => Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.Space);

    protected virtual void Start()
    {
        
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
        _shootTimer = shootDelay;
    }
}
