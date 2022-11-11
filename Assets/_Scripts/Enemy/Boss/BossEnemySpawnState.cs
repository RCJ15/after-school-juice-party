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

    }

    public override void Die()
    {

    }
}
