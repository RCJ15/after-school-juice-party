using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// 
/// </summary>
public class WaveText : MonoBehaviour
{
    public static WaveText Instance;

    [SerializeField] private TMP_Text waveNameText;
    private TMP_Text _waveText;

    private Animator _anim;

    public bool CanPlayMusic = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _waveText = GetComponent<TMP_Text>();
    }
    
    private void Update()
    {
        
    }

    public void Appear(int wave, string waveName)
    {
        _waveText.text = "Wave " + wave.ToString();
        waveNameText.text = waveName;

        _anim.SetTrigger("Appear");
    }

    public void StartMusic()
    {
        if (!CanPlayMusic)
        {
            return;
        }

        SoundManager.PlaySound("Loose Stair");

        MusicPlayer.PlaySong();

        CanPlayMusic = false;
    }
}
