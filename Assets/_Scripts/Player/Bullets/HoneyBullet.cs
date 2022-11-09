using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyBullet : GravityBullet
{
    [SerializeField] private float delay = 0.5f;
    [SerializeField] private float timeBoosted = 3f;

    protected override bool CanBeBoostedByHoney => false;

    protected override void Update()
    {
        base.Update();

        if (delay > 0)
        {
            delay -= Time.deltaTime;
        }
    }

    protected override void OnCollideWithPlayer(Collider2D col)
    {
        if (delay <= 0)
        {
            shootManager.HoneyTimer = timeBoosted;
            Die();

            CameraEffects.Flash(2, Color.yellow);
        }

        base.OnCollideWithPlayer(col);
    }
}
