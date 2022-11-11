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

    public void Idle()
    {
        Boss.Idle();
    }

    public virtual void UpdateStage(int stage)
    {

    }

    public void PlayGiantLaserPrepareSFX()
    {
        SoundManager.PlaySound("Giant Laser Prepare");
    }

    public void PlayGiantLaserFireSFX()
    {
        SoundManager.PlaySound("Giant Laser Fire");
    }

    public void PlayExplosionSFX()
    {
        SoundManager.PlaySound("Explosion", Random.Range(0.8f, 1.2f), 0.5f);
    }

    public void PlayClingSFX()
    {
        SoundManager.PlaySound("Cling");
    }
}
