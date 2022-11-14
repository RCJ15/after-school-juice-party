using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WAVYMusic;

/// <summary>
/// 
/// </summary>
public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance;

    private static WAVYSong _currentSong;
    [SerializeField] private WAVYSong[] gameSongs;
    [SerializeField] private WAVYSong bossSong;

    private int _gameSongsLength;
    private int _gameSong;

    private void Awake()
    {
        _gameSongsLength = gameSongs.Length;
        Instance = this;
    }

    public static void PlayBossSong()
    {
        _currentSong?.Stop();

        _currentSong = Instance.bossSong;
        _currentSong.Play();
    }

    public static void StopSong(float fadeDuration = 0)
    {
        _currentSong?.Stop(fadeDuration);
    }

    public static void PlaySong()
    {
        PlaySong(Instance._gameSong++ % Instance._gameSongsLength);
    }

    public static void PlaySong(int index)
    {
        _currentSong?.Stop();

        _currentSong = Instance.gameSongs[index];
        _currentSong.Play();
    }
}
