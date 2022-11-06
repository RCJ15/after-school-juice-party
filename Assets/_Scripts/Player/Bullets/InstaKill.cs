using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstaKill : Bullet
{// Save enemys killed now for the boss battel, can not be used then!
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))//|| collision.CompareTag("Wall")) // Hit enemy or wall
        {
            SpawnInstaKillEnemys.savedEnemys.Add((collision.gameObject)); // Save enemy for later
        }
    }

}