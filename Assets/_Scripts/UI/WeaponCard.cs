using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/// <summary>
/// A single weapon card that the player can equip
/// </summary>
public class WeaponCard : MonoBehaviour // - Ruben
{
    [SerializeField] private TMP_Text nameText;

    [SerializeField] private Image sprite;
    [SerializeField] private TMP_Text atkStat;
    [SerializeField] private TMP_Text utilStat;
    [SerializeField] private TMP_Text rangeStat;
    [SerializeField] private TMP_Text shootStat;

    private WeaponHUD _hud;
    private Animator _anim;
    private PlayerShoot _weapon;

    private void Awake()
    {
        _hud = WeaponHUD.Instance;

        _anim = GetComponentInChildren<Animator>();
    }

    public void Set(PlayerShoot weapon, bool doAnim = true)
    {
        _weapon = weapon;

        if (doAnim)
        {
            _anim.SetTrigger("Change");

            Invoke(nameof(UpdateCard), 0.175f);
        }
        else
        {
            UpdateCard();
        }
    }

    public void PlaySpawnAnim()
    {
        _anim.SetTrigger("Spawn");
    }

    public void UpdateCard()
    {
        nameText.text = string.IsNullOrEmpty(_weapon.DisplayName) ? _weapon.name : _weapon.DisplayName;
        nameText.colorGradient = new VertexGradient(
            _hud.TemplateGradient.topLeft,                         // Top left
            _weapon.Color * _hud.TemplateGradient.topRight,         // Top right
            _weapon.Color * _hud.TemplateGradient.bottomLeft,       // Bottom left
            _weapon.Color * _hud.TemplateGradient.bottomRight       // Bottom right
            );

        sprite.sprite = _weapon.Sprite;
        atkStat.text = GetStatString(_weapon.AttackStat);
        utilStat.text = GetStatString(_weapon.UtilityStat);
        rangeStat.text = GetStatString(_weapon.RangeStat);

        if (_weapon.shoots <= 0) // We have unlimited shoots
        {
            shootStat.gameObject.SetActive(false);
        }
        else
        {
            shootStat.gameObject.SetActive(true);

            shootStat.text = (_weapon.shoots - _weapon.timesShot).ToString();
        }
    }

    public VertexGradient GetGradient() => nameText.colorGradient;

    public static string GetStatString(int stat)
    {
        return stat <= -1 ? "?" : stat.ToString();
    }
}