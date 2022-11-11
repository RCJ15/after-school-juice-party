using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The regular shoot type that will just spawn a single prefab.
/// </summary> - Ruben
public class RegularShoot : PlayerShoot
{
    [Header("Regular Shoot")]
    [SerializeField] protected GameObject bullet;

    protected override void Shoot()
    {
        base.Shoot();
        if (timesShot < shoots || shoots == 0) // Player has used all their bullets
        {
            Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);
        }
    }
}