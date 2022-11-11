using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class BossBulletSpamState : BossState
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float startAngle;
    [SerializeField] private Vector2 randomOffset;

    [Space]
    [SerializeField] private float[] fireRates;

    private float _timer;

    private bool _spamming;

    private void OnEnable()
    {
        Animate("Bullet Spam");

        _timer = 0;
    }

    public void StartBossBulletSpam()
    {
        _spamming = true;
    }

    public void StopBossBulletSpam()
    {
        _spamming = false;
    }

    public override void Die()
    {
        _spamming = false;
        _timer = 0;
    }

    protected override void Update()
    {
        base.Update();

        if (!_spamming)
        {
            return;
        }

        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            return;
        }

        Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, startAngle + Random.Range(randomOffset.x, randomOffset.y)));

        _timer = fireRates[Stage - 1];
    }
}
