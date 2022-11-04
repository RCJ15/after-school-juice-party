using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Explodes
/// </summary> - Ruben
public class Explosion : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private HashSet<GameObject> hitEnemies = new HashSet<GameObject>();
    public HashSet<GameObject> HitEnemies { get => hitEnemies; set => hitEnemies = value; }
    public float Damage { get => damage; set => damage = value; }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (hitEnemies.Contains(col.gameObject) || !col.CompareTag("Enemy") || !col.TryGetComponent(out Enemy enemy))
        {
            return;
        }

        hitEnemies.Add(col.gameObject);

        // TEMPORARY
        enemy.GetComponent<DummyEnemy>().Hurt(damage);
    }
}
