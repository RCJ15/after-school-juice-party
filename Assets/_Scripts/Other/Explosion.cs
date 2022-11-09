using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Explodes
/// </summary> - Ruben
public class Explosion : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float speed = 1;

    [Header("Juice")]
    [SerializeField] private float shakeIntesity;
    [SerializeField] private float shakeDuration;
    [Space]
    [SerializeField] private float flashHoldDuration;
    [SerializeField] private float flashDuration;
    [SerializeField] private Color flashColor = Color.white;
    [Space]
    [SerializeField] private float zoomAmount = 68;
    [SerializeField] private float zoomDuration = 0.1f;

    private Animator _anim;

    private HashSet<GameObject> _hitEnemies = new HashSet<GameObject>();
    public HashSet<GameObject> HitEnemies { get => _hitEnemies; set => _hitEnemies = value; }
    public float Damage { get => damage; set => damage = value; }

    private void Awake()
    {
        _anim = GetComponent<Animator>();

        _anim.SetFloat("Speed", speed);

        Destroy(gameObject, 1 / speed);
    }

    private void Start()
    {
        if (shakeIntesity > 0 && shakeDuration > 0)
        {
            CameraEffects.Shake(shakeIntesity, shakeDuration);
        }

        if (flashDuration > 0 || flashHoldDuration > 0)
        {
            CameraEffects.Flash(flashHoldDuration, flashDuration, flashColor);
        }

        if (zoomAmount > 0 && zoomDuration > 0)
        {
            CameraEffects.Zoom(zoomAmount, zoomDuration, Vector3.zero);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (_hitEnemies.Contains(col.gameObject) || !col.CompareTag("Enemy"))
        {
            return;
        }

        _hitEnemies.Add(col.gameObject);
        col.TryGetComponent(out Enemy enemy);
        // TEMPORARY
        try
        {
            col.GetComponent<Enemy>().Hurt(damage);
                        enemy.GetComponent<DummyEnemy>().Hurt(damage);
        }
        catch (System.Exception)
        {

        }
    }

    public void Explode(float damage, Vector3 scale)
    {
        Explode(damage);

        transform.localScale = scale;
    }

    public void Explode(float damage)
    {
        Damage = damage;
        transform.SetParent(null);
        gameObject.SetActive(true);
    }
}