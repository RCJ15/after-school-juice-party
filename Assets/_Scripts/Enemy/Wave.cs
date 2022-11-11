using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    
    [Serializable]
    public class MiniWave
    {
        public EnemyType[] Enemies;
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
}