using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Does random things!
/// </summary> - Ruben
public class CodeBullet : Bullet
{
    [SerializeField] protected ParticleSystem trailParticles;
    [SerializeField] protected Color flashColor = Color.green;
    [SerializeField] protected float flashDuration = 0.15f;

    [Header("Code Bullet")]
    [SerializeField] protected float enemySurviveChance = 0.3f;
    [SerializeField] protected int maxTimesSurvive = 3;
    protected int timesSurvived;

    [Header("Rng")]
    [SerializeField] protected Vector2 rngDamage = new Vector2(4f, 8f);
    [SerializeField] protected Vector2 rngSize = new Vector2(0.5f, 2.5f);
    [SerializeField] protected Vector2 rngSpeed = new Vector2(3, 7);
    [SerializeField] protected Vector2 rngRotation = new Vector2(-90, 90);

    private Vector2 _startSize;

    protected override void Start()
    {
        base.Start();

        _startSize = transform.localScale;
        trail.widthMultiplier = _startSize.magnitude / 2;

        RandomizeStats(false);

        CameraEffects.Flash(flashDuration, flashColor);
    }

    protected override bool OnCollideWithEnemy(Collider2D col, Enemy enemy)
    {
        HurtEnemy(enemy);

        if (timesSurvived >= maxTimesSurvive || Random.value > enemySurviveChance)
        {
            Die();
            return true;
        }

        if (!string.IsNullOrEmpty(deathSoundEffect))
        {
            SoundManager.PlaySound(deathSoundEffect);
        }

        RandomizeStats(true);
        timesSurvived++;

        deathParticles.Play();

        return true;
    }

    protected void RandomizeStats(bool randomizeRotation)
    {
        damage = Random.Range(rngDamage.x, rngDamage.y);
        float scale = Random.Range(rngSize.x, rngSize.y);
        transform.localScale = _startSize * scale;
        trail.widthMultiplier = scale / 2;
        speed = Random.Range(rngSpeed.x, rngSpeed.y);

        if (randomizeRotation)
        {
            transform.Rotate(Vector3.forward, Random.Range(rngRotation.x, rngRotation.y));
        }
    }

    protected override void Die()
    {
        base.Die();

        trailParticles.Stop();

        DetachParticleSystem(trailParticles);
    }
}
