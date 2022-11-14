using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class BossEnemySpawnState : BossState
{
    [SerializeField] private DataClass[] enemies;

    [SerializeField] private int[] amount;

    private void OnEnable()
    {
        Animate("Spawn Enemy");
    }

    public void SpawnEnemy()
    {
        if (Boss.Dead)
        {
            return;
        }

        int rng = Random.Range(0, enemies.Length);

        for (int i = 0; i < amount[Stage - 1]; i++)
        {
            Enemy enemy = enemies[Stage - 1].Enemies[rng++ % enemies[Stage - 1].Enemies.Length];

            Enemy newEnemy = Instantiate(enemy, transform.position, Quaternion.identity);
            newEnemy.GivePoints = false;
            newEnemy.SpawnedByBossOrSpawner = true;
        }

        SoundManager.PlaySound("Space Zipper");
    }

    public override void Die()
    {

    }

    [System.Serializable]
    public class DataClass
    {
        public Enemy[] Enemies;
    }
}
