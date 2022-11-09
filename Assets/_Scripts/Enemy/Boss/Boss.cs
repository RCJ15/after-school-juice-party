using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Boss : MonoBehaviour
{
    [SerializeField] private BossState idleState;

    [Space]
    [SerializeField] private BossState[] attackStates;

    [SerializeField] private float speedLerp;
    public float TargetSpeed { get; set; }
    
    private void Awake()
    {
        foreach (BossState attackState in attackStates)
        {

            attackState.enabled = false;
        }
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }
}
