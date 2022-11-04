using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains important info about the game and handles the game loop. <para/>
/// Use: <see cref="Instance"/>
/// </summary> - Ruben
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public LayerMask WallLayer;
    public LayerMask PlayerLayer;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
