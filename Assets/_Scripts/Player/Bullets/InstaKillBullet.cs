using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstaKillBullet : Bullet
{
    protected override bool OnCollideWithEnemy(Collider2D col, Enemy enemy)
    {
        SpawnInstaKillEnemies.savedEnemies.Add((enemy.gameObject)); // Save enemy for trolling :)

        return true;
    }
}