using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
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
