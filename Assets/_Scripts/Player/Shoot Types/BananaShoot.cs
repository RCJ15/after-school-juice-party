using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The shoot type for the banana flavor. Will limit how many bullets can be present at a time.
/// </summary> - Ruben
public class BananaShoot : PlayerShoot
{
    [Header("Banana Shoot")]
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
