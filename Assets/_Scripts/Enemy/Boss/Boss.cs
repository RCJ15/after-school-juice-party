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

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
