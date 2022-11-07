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
    [SerializeField] private TMP_Text cvrgStat;

    private WeaponHUD _hud;
    private Animator _anim;

    private void Awake()
    {
        _hud = WeaponHUD.Instance;

        _anim = GetComponentInChildren<Animator>();
    }

    public void Set(PlayerShoot weapon)
    {
        nameText.text = weapon.name;
        nameText.colorGradient = new VertexGradient(
            _hud.TemplateGradient.topLeft,                         // Top left
            weapon.Color * _hud.TemplateGradient.topRight,         // Top right
            weapon.Color * _hud.TemplateGradient.bottomLeft,       // Bottom left
            weapon.Color * _hud.TemplateGradient.bottomRight       // Bottom right
            );

        sprite.sprite = weapon.Sprite;
        atkStat.text = GetStatString(weapon.AttackStat);
        utilStat.text = GetStatString(weapon.UtilityStat);
        cvrgStat.text = GetStatString(weapon.CoverageStat);
    }

    public void PlaySpawnAnim()
    {
        _anim.SetTrigger("Spawn");
    }

    public VertexGradient GetGradient() => nameText.colorGradient;

    public static string GetStatString(int stat)
    {
        return stat <= -1 ? "?" : stat.ToString();
    }
}
