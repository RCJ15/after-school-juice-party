using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class BossEnemySpawnState : BossState
{
    [SerializeField] private GameObject[] enemies;

    [SerializeField] private int[] amount;

    private void OnEnable()
    {
        Anim.SetTrigger("Spawn Enemy");
    }

    public void SpawnEnemy()
    {

    }
}
