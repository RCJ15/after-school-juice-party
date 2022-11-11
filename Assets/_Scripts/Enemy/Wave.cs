using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    [SerializeField] private string waveName;
    public string WaveName => waveName;

    [Space]
    [SerializeField] private MiniWave[] waves;
    public MiniWave[] Waves => waves;

    [SerializeField] private bool spawnBoss;
    [SerializeField] [Range(1, 5)] private int bossStage = 1;

    public bool SpawnBoss => spawnBoss;
    public int BossStage => bossStage;

    [Serializable]
    public class MiniWave
    {
        public EnemyType[] Enemies;
        public int Amount;
        public float Time;
        public float Delay;
    }
}

[Serializable]
public enum EnemyType
{
    Regular,
    Shooter,
    Soldier,
    Tank,
    ZigZag,
    Mini,
    Spawner,
}