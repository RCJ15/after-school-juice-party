using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class BossState : MonoBehaviour
{
    public Boss Boss { get; set;}
    public Animator Anim { get; set; }
    public int Stage => Boss.Stage;

    protected float TargetSpeed { get => Boss.TargetSpeed; set => Boss.TargetSpeed = value; }

    protected virtual void Start()
    {
        TargetSpeed = 0;
    }

    protected virtual void Update()
    {

    }

    public virtual void UpdateStage(int stage)
    {

    }

    public virtual void Die()
    {

    }

    protected void Animate(string animName)
    {
        Boss.Animate(animName);
    }
}
