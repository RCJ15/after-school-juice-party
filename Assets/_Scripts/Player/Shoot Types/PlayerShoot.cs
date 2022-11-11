using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The player shoot script
/// </summary> - Ruben
public class PlayerShoot : MonoBehaviour
{
    [SerializeField] protected float shootDelay;
    protected float _shootTimer;
    public int shoots = 0;
    [HideInInspector] public int timesShot = 0;

    [SerializeField] protected Transform spawnPoint;

    [Header("Stats")]
    public Sprite Sprite;
    [TextArea(5, 5)]
    public string Description = "Sample Text";
    public Color Color = Color.white;

    [Space(5)]
    [Range(-1, 5)]
    public int AttackStat = 0;
    [Range(-1, 5)]
    public int UtilityStat = 0;
    [Range(-1, 5)]
    public int RangeStat = 0;
    public DamageType[] DmgTypes = new DamageType[] { DamageType.Single };
    public string Special;

    [Header("Juice")]
    [SerializeField] protected string shootSoundEffect;
    [SerializeField] protected ParticleSystem shootEffect;
    [SerializeField] protected bool animatePlayer = true;
    [SerializeField] protected float shakeIntensity = 0.1f;
    [SerializeField] protected float shakeDuration = 0.05f;

    protected Animator _playerAnim;
    [HideInInspector] public int weaponIndex;
    [HideInInspector] public WeaponCard weaponCard;

    protected bool ShootKeyDown => Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space);
    protected bool ShootKey => Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space);
    protected bool ShootKeyUp => Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.Space);

    protected virtual void Start()
    {
        _playerAnim = PlayerMove.Instance.GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        if (_shootTimer > 0)
        {
            _shootTimer -= Time.deltaTime;
            return;
        }

        // Shoot
        if (ShootKey)
        {
            Shoot();
        }
    }

    protected virtual void Shoot()
    {
        timesShot++;
        weaponCard.UpdateCard();
        if (timesShot >= shoots && shoots != 0) // Player has used all their bullets
        {
            FindObjectOfType<PlayerShootManager>().ChangeWeapon(weaponIndex, 0);
            return;
        }

        if (shootEffect != null)
        {
            shootEffect.Play();
        }

        if (animatePlayer)
        {
            DoPlayerShootAnim();
        }

        if (shakeIntensity > 0 && shakeDuration > 0)
        {
            CameraEffects.Shake(shakeIntensity, shakeDuration);
        }

        _shootTimer = shootDelay;

        if (!string.IsNullOrEmpty(shootSoundEffect))
        {
            SoundManager.PlaySound(shootSoundEffect);
        }
    }

    protected virtual void DoPlayerShootAnim()
    {
        _playerAnim.SetTrigger("Shoot");
    }

    [Serializable]
    public enum DamageType
    {
        Single,
        Pierce,
        Area,
        Random,
    }
}