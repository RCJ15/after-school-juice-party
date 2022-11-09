using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleShoot : PlayerShoot
{
    [SerializeField] private GameObject appleSeed;
    [SerializeField] private float angle;
    [SerializeField] private float middleSpeed;

    protected override void Shoot()
    {
        GameObject GetSeed(float angle)
        {
            return Instantiate(appleSeed, spawnPoint.position, spawnPoint.rotation * Quaternion.Euler(0, 0, angle));
        }

        // Spawn 3 apple seeds at different angles
        GetSeed(-angle).GetComponent<AppleBullet>().curveAngle = 1;
        GetSeed(0).GetComponent<AppleBullet>().Speed = middleSpeed;
        GetSeed(angle).GetComponent<AppleBullet>().curveAngle = -1;

        base.Shoot();
    }
}
