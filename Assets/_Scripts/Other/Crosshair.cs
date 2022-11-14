using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Crosshair
/// </summary> - Ruben
public class Crosshair : MonoBehaviour
{
    public Transform Target;
    [SerializeField] private string soundEffect;

    private void Update()
    {
        if (Target == null)
        {
            return;
        }

        transform.position = Target.position;
    }

    public void PlaySFX()
    {
        if (string.IsNullOrEmpty(soundEffect))
        {
            return;
        }

        SoundManager.PlaySound(soundEffect);
    }
}
