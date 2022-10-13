using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The player shoot script
/// </summary> - Ruben
public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float shootDelay;
    private float _shootTimer;

    [SerializeField] private Transform spawnPoint;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (_shootTimer > 0)
        {
            _shootTimer -= Time.deltaTime;
            return;
        }

        // Shoot
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        _shootTimer = shootDelay;

        Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);
    }
}
