using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Crosshair
/// </summary> - Ruben
public class Crosshair : MonoBehaviour
{
    public Transform Target;

    private void Update()
    {
        transform.position = Target.position;
    }
}
