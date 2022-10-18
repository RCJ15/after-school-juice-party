using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class BananaShoot : PlayerShoot
{
    [SerializeField] protected int maxBananas = 5;
    private int _spawnedBananas;

    [SerializeField] protected GameObject bullet;

    protected override void Update()
    {
        if (_shootTimer > 0)
        {
            _shootTimer -= Time.deltaTime;
            return;
        }

        if (_spawnedBananas >= maxBananas)
        {
            return;
        }

        if (ShootKey)
        {
            Shoot();
        }
    }

    protected override void Shoot()
    {
        base.Shoot();

        BananaBullet banana = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation).GetComponent<BananaBullet>();

        banana.Shoot = this;

        _spawnedBananas++;
    }

    public void BananaDies(BananaBullet banana)
    {
        _spawnedBananas--;
    }
}
