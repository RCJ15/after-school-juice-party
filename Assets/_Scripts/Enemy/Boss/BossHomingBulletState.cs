using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class BossHomingBulletState : BossState
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject homingBullet;

    [Space]
    [SerializeField] private Vector2[] explosionSize;

    private List<ExplosionBullet> _bullets = new List<ExplosionBullet>();

    private void OnEnable()
    {
        Animate("Homing Bullet");
    }

    public void SpawnHomingBullet()
    {
        ExplosionBullet bullet = Instantiate(homingBullet, spawnPoint.position, spawnPoint.rotation).GetComponent<ExplosionBullet>();

        bullet.ExplosionSize = explosionSize[Stage - 1];

        _bullets.Add(bullet);
    }

    public override void Die()
    {
        foreach (ExplosionBullet bullet in _bullets)
        {
            if (bullet == null)
            {
                continue;
            }

            Destroy(bullet.gameObject);
        }

        _bullets = new List<ExplosionBullet>();
    }
}
