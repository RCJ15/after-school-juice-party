using UnityEngine;
using TMPro;

public class PauseScreen : MonoBehaviour
{
    public static PauseScreen Instance;

    public static bool CanPause { get => Instance._canPause; set => Instance._canPause = value; }
    private bool _canPause = true;

    [SerializeField] GameObject panel;
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text text;
    [SerializeField] TransitionController transitionController;

    float _timeElapsed = 0;
    string _currentLevelName ="";

    private void Awake()
    {
        Instance = this;
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
                titleText.text = _currentLevelName; // Set the title 
                text.text = $"Name: {HighScore.PlayerName} \nScore: {Score.PlayerScore} \nTime: {Mathf.Round(_timeElapsed * 10) / 10} "; // Set some stats
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
