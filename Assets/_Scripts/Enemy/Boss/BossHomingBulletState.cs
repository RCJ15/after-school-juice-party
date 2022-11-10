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

    private void OnEnable()
    {
        Anim.SetTrigger("Homing Bullet");
    }

    public void SpawnHomingBullet()
    {
        Instantiate(homingBullet, spawnPoint.position, spawnPoint.rotation);
    }
}
