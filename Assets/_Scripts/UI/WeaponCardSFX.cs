using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class WeaponCardSFX : MonoBehaviour
{
    public void PlaySpawnSFX()
    {
        SoundManager.PlaySound("Receive Card", 1, 1);
    }
    public void PlayLandSFX()
    {
        SoundManager.PlaySound("Receive Card Impact", 1, 1);
        SoundManager.PlaySound("Powerup", 1, 1);
    }
}
