using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The regular shoot type that will just spawn a single prefab.
/// </summary> - Ruben
public class RegularShoot : PlayerShoot
{
    [SerializeField] protected GameObject bullet;

    protected override void Shoot()
    {
        base.Shoot();

        Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);
    }
}
