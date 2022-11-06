using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Explodes
/// </summary> - Ruben
public class Explosion : MonoBehaviour
{
    [SerializeField] private float damage;

    [Header("Juice")]
    [SerializeField] private float shakeIntesity;
    [SerializeField] private float shakeDuration;
    [Space]
    [SerializeField] private float flashDuration;
    [SerializeField] private Color flashColor = Color.white;
    [Space]
    [SerializeField] private float zoomAmount = 68;
    [SerializeField] private float zoomDuration = 0.1f;

    private HashSet<GameObject> _hitEnemies = new HashSet<GameObject>();
    public HashSet<GameObject> HitEnemies { get => _hitEnemies; set => _hitEnemies = value; }
    public float Damage { get => damage; set => damage = value; }

    private void Start()
    {
        CameraEffects.Shake(shakeIntesity, shakeDuration);
        CameraEffects.Flash(flashDuration, flashColor);
        CameraEffects.Zoom(zoomAmount, zoomDuration, Vector3.zero);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (_hitEnemies.Contains(col.gameObject) || !col.CompareTag("Enemy") || !col.TryGetComponent(out Enemy enemy))
        {
            return;
        }

        _hitEnemies.Add(col.gameObject);

        // TEMPORARY
        enemy.GetComponent<DummyEnemy>().Hurt(damage);
    }
}
