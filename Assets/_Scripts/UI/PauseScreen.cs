using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text text;

    float _attempt = 0, _timeElapsed = 0;
    string _currentLevelName ="";
    string _currentWeapon ="";
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            // Changed from "panel.active" to "panel.activeSelf" - Ruben
            // (It does exatly the same thing but doesn't spam any warning in the console)
            if (panel.activeSelf)
            {
                panel.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                titleText.text = _currentLevelName; // Set the title 
                text.text = $"Name: {HighScore.PlayerName} \nScore: {Score.PlayerScore} \nWeapon equipped: {_currentWeapon} \nAttempt: {_attempt} \nTime: {_timeElapsed} "; // Set some stats
                panel.SetActive(true);
                Time.timeScale = 0;
            }
        } 
    }

    public void Resume()
    {
        panel.SetActive(false);
        Time.timeScale = 1;
    }
    public void Option()
    {

    }
    public void MainMenu()
    {

    }
}
