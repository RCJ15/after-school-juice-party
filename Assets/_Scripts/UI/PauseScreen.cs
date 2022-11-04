using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text text;


    float _Attempt = 0, _TimeElapsed = 0;
    string _CurrentLevelName ="";
    string _CurrentWeapond ="";
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) 
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
                titleText.text = _CurrentLevelName; // Set the title 
                text.text = $"Name: {HighScore.playerName} \nScore: {Score.playerScore} \nWeapond equiped: {_CurrentWeapond} \nAttempt: {_Attempt} \nTime elapsed: {_TimeElapsed} "; // Set some stats
                                panel.SetActive(true);
                Time.timeScale = 0;
            }
        } 
    }

    public void  Resume()
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
