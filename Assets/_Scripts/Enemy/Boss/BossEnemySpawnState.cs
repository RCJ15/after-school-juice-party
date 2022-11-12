using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class BossEnemySpawnState : BossState
{
    [SerializeField] private Enemy[] enemies;

    [SerializeField] private int[] amount;

    private void OnEnable()
    {
        Animate("Spawn Enemy");
    }

    public void SpawnEnemy()
    {
        int rng = Random.Range(0, enemies.Length);

        for (int i = 0; i < amount[Stage - 1]; i++)
        {
            Enemy enemy = enemies[rng++ % enemies.Length];

            Instantiate(enemy, transform.position, Quaternion.identity).GivePoints = false;
        }

        SoundManager.PlaySound("Space Zipper");
    }

    public override void Die()
    {

    }
}
