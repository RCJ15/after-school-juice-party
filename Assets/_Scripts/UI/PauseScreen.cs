using System;
using UnityEngine;
using TMPro;

public class PauseScreen : MonoBehaviour
{
    public static PauseScreen Instance;

    public static bool CanPause { get => Instance._canPause; set => Instance._canPause = value; }
    private bool _canPause = false;

    [SerializeField] GameObject panel;
    [SerializeField] TMP_Text titleText;
    [SerializeField] TransitionController transitionController;

    float _timeElapsed = 0;

    WaveManager _waveManager;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _waveManager = WaveManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (_switching)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && _canPause) 
        {
            // Changed from "panel.active" to "panel.activeSelf" - Ruben
            // (It does exatly the same thing but doesn't spam any warning in the console)
            if (panel.activeSelf)
            {
                Resume();
            }
            else
            {
                titleText.text = $"Wave {_waveManager.CurrentWave} - {_waveManager.WaveName}"; // Set the title 
                panel.SetActive(true);
                Time.timeScale = 0;
            }
        }

        _timeElapsed += Time.deltaTime;
    }

    public void Resume()
    {
        if (_switching)
        {
            return;
        }

        panel.SetActive(false);
        Time.timeScale = 1;
    }

    private bool _switching;
    public void MainMenu()
    {
        if (_switching)
        {
            return;
        }

        _switching = true;

        transitionController.Transition(0.75f, 0.5f, 0.75f, SceneChange, true);

        void SceneChange(){
            UnityEngine.SceneManagement.SceneManager.LoadScene("Start");
        }

        MusicPlayer.StopSong(1);
    }
}
