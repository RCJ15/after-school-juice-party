using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class BossAnimEvents : MonoBehaviour
{
    public Boss Boss { get; set; }

    [SerializeField] private float giantLaserPrepareVol = 0.5f;
    [SerializeField] private float giantLaserFireVol = 0.3f;
    [SerializeField] private float explosionVol = 0.3f;
    [SerializeField] private float clingVol = 0.5f;
    [SerializeField] private float glassDragVol = 0.5f;

    public void StartBoss()
    {
        Boss.StartBoss();
    }

    public void Idle()
    {
        Boss.Idle();
    }

    private float Pitch => Random.Range(0.8f, 1.2f);

    public void PlayGiantLaserPrepareSFX()
    {
        SoundManager.PlaySound("Giant Laser Prepare", Pitch, giantLaserPrepareVol);
    }

    public void PlayGiantLaserFireSFX()
    {
        SoundManager.PlaySound("Giant Laser Fire", Pitch, giantLaserFireVol);
    }

    public void PlayExplosionSFX()
    {
        SoundManager.PlaySound("Explosion", Pitch, explosionVol);
    }

    public void PlayClingSFX()
    {
        SoundManager.PlaySound("Cling", Pitch, clingVol);
    }

    public void PlayGlassDragSFX()
    {
        SoundManager.PlaySound("Glass Drag", Pitch, glassDragVol);
    }

    public void Die()
    {

    }
}
