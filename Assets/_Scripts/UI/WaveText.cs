using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class WaveText : MonoBehaviour
{
    public static WaveText Instance;

    [SerializeField] private TMPro.TMP_Text waveNameText;

    private Animator _anim;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }
    
    private void Update()
    {
        
    }

    public void Appear(string waveName)
    {
        waveNameText.text = waveName;
    }
}
