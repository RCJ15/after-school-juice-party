using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class BossState : MonoBehaviour
{
    public Boss Boss { get; set;}

    protected float TargetSpeed { get => Boss.TargetSpeed; set => Boss.TargetSpeed = value; }

    protected virtual void Start()
    {
        TargetSpeed = 0;
    }

    protected virtual void Update()
    {

    }
}
