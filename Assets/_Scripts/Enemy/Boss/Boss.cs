using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 
/// </summary>
public class Boss : MonoBehaviour
{
    [SerializeField] private BossState idleState;

    [Space]
    [SerializeField] private BossState[] attackStates;

    [SerializeField] private float speedLerp;
    [HideInInspector] public float LerpMultiplier = 1;

    private BossState _currentState;

    private Rigidbody2D _rb;

    public float TargetSpeed { get; set; }
    public float CurrentSpeed { get; set; }
    
    private void Awake()
    {
        _currentState = idleState;
        idleState.Boss = this;

        foreach (BossState attackState in attackStates)
        {
            attackState.Boss = this;
            attackState.enabled = false;
        }

        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        CurrentSpeed = Mathf.Lerp(CurrentSpeed, TargetSpeed, speedLerp * LerpMultiplier);

        _rb.velocity = new Vector2(CurrentSpeed, 0);
    }

    public void Attack()
    {
        idleState.enabled = false;

        //_currentState = attackStates[Random.Range(0, attackStates.Length)];
        _currentState = attackStates[1];
        _currentState.enabled = true;

        Debug.Log(_currentState);
    }

    public void Idle()
    {
        _currentState.enabled = false;

        _currentState = idleState;
        _currentState.enabled = true;
    }
}
