using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WAVYMusic;

/// <summary>
/// 
/// </summary>
public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private WAVYSong[] gameSongs;
    [SerializeField] private WAVYSong bossSong;

    private void Start()
    {
        gameSongs[0].Play();
    }
}
