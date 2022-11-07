using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

/// <summary>
/// 
/// </summary>
public class WeaponPickupPopup : MonoBehaviour // - Ruben
{
    public static WeaponPickupPopup Instance;

    [SerializeField] private TMP_Text weaponName;
    private string _startWeaponNameText;
    private VertexGradient _startWeaponNameGradient;

    [SerializeField] private TMP_Text attackStat;
    private string _startAttackStatText;

    [SerializeField] private TMP_Text utilityStat;
    private string _startUtilityStatText;

    [SerializeField] private TMP_Text coverageStat;
    private string _startCoverageStatText;

    [SerializeField] private TMP_Text description;
    private string _startDescriptionText;

    #region OLD CODE
    [SerializeField] private TMP_Text attackType;
    private string _startAttackTypeText;

    [SerializeField] private TMP_Text special;
    private string _startSepcialText;
    #endregion

    private Animator _anim;

    private void Awake()
    {
        Instance = this;

        _startWeaponNameText = weaponName.text;
        _startWeaponNameGradient = weaponName.colorGradient;

        _startAttackStatText = attackStat.text;
        _startUtilityStatText = utilityStat.text;
        _startCoverageStatText = coverageStat.text;
        _startDescriptionText = description.text;

        #region OLD CODE
        if (attackType != null)
        {
            _startAttackTypeText = attackType.text;
        }
        if (special != null)
        {
            _startSepcialText = special.text;
        }
        #endregion

        _anim = GetComponent<Animator>();
    }

    public void Popup(PlayerShoot weapon)
    {
        // Name
        weaponName.text = string.Format(_startWeaponNameText, weapon.name);
        weaponName.colorGradient = new VertexGradient(
            _startWeaponNameGradient.topLeft,                       // Top left
            weapon.Color * _startWeaponNameGradient.topRight,       // Top right
            weapon.Color * _startWeaponNameGradient.bottomLeft,     // Bottom left
            weapon.Color * _startWeaponNameGradient.bottomRight     // Bottom right
            );

        // Stats
        attackStat.text = string.Format(_startAttackStatText, GetStatString(weapon.AttackStat));
        utilityStat.text = string.Format(_startUtilityStatText, GetStatString(weapon.UtilityStat));
        coverageStat.text = string.Format(_startCoverageStatText, GetStatString(weapon.CoverageStat));

        // Description
        description.text = string.Format(_startDescriptionText, weapon.Description);

        #region OLD CODE
        // Attack type
        if (attackType != null)
        {
            StringBuilder stringBuilder = new StringBuilder(null);
            int length = weapon.DmgTypes.Length;

            if (length > 0)
            {
                if (length == 1)
                {
                    // Format: "{0}"
                    stringBuilder.Append(weapon.DmgTypes[0]);
                }
                else
                {
                    // Format: "{0}, {1} & {2}" etc...
                    for (int i = 0; i < length; i++)
                    {
                        stringBuilder.Append(weapon.DmgTypes[i]);

                        if (i == length - 2)
                        {
                            stringBuilder.Append(" & ");
                        }
                        else if (i != length - 1)
                        {
                            stringBuilder.Append(", ");
                        }
                    }
                }
            }

            string dmgTypeText = stringBuilder.ToString();

            attackType.text = string.Format(_startAttackTypeText, dmgTypeText);
        }

        if (special != null)
        {
            // Special
            bool hasSpecial = !string.IsNullOrEmpty(weapon.Special);

            special.gameObject.SetActive(hasSpecial);

            if (hasSpecial)
            {
                special.text = string.Format(_startSepcialText, weapon.Special);
            }
        }
        #endregion

        // Animate
        _anim.SetTrigger("Popup");
    }

    public static string GetStatString(int stat)
    {
        return stat <= -1 ? "???" : stat.ToString();
    }
}
