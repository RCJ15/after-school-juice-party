using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Same as the RegularShoot but will choose a random shoot from a prefab array.
/// </summary> - Ruben
public class RandomShoot : PlayerShoot
{
    [SerializeField] protected GameObject[] bullets;
    protected int _bulletsLength;

    protected override void Start()
    {
        base.Start();

        _bulletsLength = bullets.Length;
    }

    protected override void Shoot()
    {
        base.Shoot();

        Instantiate(bullets[Random.Range(0, _bulletsLength)], spawnPoint.position, spawnPoint.rotation);
    }

}
