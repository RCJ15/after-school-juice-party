using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class HighScore : MonoBehaviour
{
    [Header("Highscore")]
    [SerializeField] GameObject highscorePanel;
    [SerializeField] GameObject content;
    [SerializeField] GameObject playerScoreTemplate;
    [Space]
    [Header("Get player name")]
    [SerializeField] GameObject inputName;
    [SerializeField] TMP_InputField inputNameInputField;
    [SerializeField] TMP_Text inputScoreTMPText;

    
    public static string playerName = "";
    Dictionary<string, int> highscore = new Dictionary<string, int>();
    List<GameObject> spawnedHighscore = new List<GameObject>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) // Debug
        {
            ShowHighScoreTable();
        }
        if (Input.GetKeyDown(KeyCode.R)) // Debug
        {
            ResetScore();
        }
    }

    /// <summary>
    /// Continue to the new thing
    /// </summary>
    public void Continue()
    {

    }
    /// <summary>
    /// Go back to the main menu
    /// </summary>
    public void MainMenu()
    {

    }
    /// <summary>
    /// Idk what yet :/
    /// </summary>
    public void LastButton()
    {

    }

    /// <summary>
    /// Resets the players name and score
    /// </summary>
    void ResetScore()
    {
        Score.playerScore = 0;
        playerName = "";
    }
    /// <summary>
    /// Asign players name and add to score board
    /// </summary>
    /// <param name="confirm">Confirm the players name</param>
    public void GetPlayerName(bool confirm)
    {
        inputScoreTMPText.text = Score.playerScore.ToString();

        if (confirm && playerName == "") // Set players name
        {
            playerName = inputNameInputField.text; // Get name from inputfield
            if (playerName == "" || playerName==null) // Still not set, player left it blank
            {
                playerName = "Player" + Score.playerScore; // Assign deutfult name
            }

            inputName.SetActive(false); // Hide panel
            AddToHighscore(playerName, Score.playerScore);
            PrintHighScore();
        }
        else if (playerName == "") // Player has no name yet
        {
            inputName.SetActive(true);

            Animator[] animators = inputName.GetComponentsInChildren<Animator>();

            animators[0].Play("Wobbel");
            animators[1].Play("Swing");
            animators[2].Play("InputShake");
        }
    }

    /// <summary>
    /// Add a entry to highscore.
    /// </summary>
    /// <param name="playerName">Name of player.</param>
    /// <param name="score">Score of player.</param>
    public void AddToHighscore(string playerName, int score)
    {
        highscore.Add(playerName, score); // Add entry to list
    }
    /// <summary>
    /// Show the highscore panel
    /// </summary>
    void ShowHighScoreTable()
    {
        if (highscorePanel.activeInHierarchy)
        {
            highscorePanel.SetActive(false);
            // Time.timeScale = 1;
        }
        else
        {
            highscorePanel.SetActive(true);
            //  Time.timeScale = 0;
            GetPlayerName(false);
        }
    }
    /// <summary>
    /// Prints out the highscors of players and sorts them acording to rank
    /// </summary>
    void PrintHighScore()
    {
        foreach (var item in spawnedHighscore) // Get rid of previous cards
        {
            Destroy(item);
        }
        spawnedHighscore = new List<GameObject>(); // Reset list
        foreach (var score in highscore) // Add every highscore
        {
            GameObject newScore = Instantiate(playerScoreTemplate, content.transform); // Add new section
            newScore.SetActive(true);
            TMP_Text[] txts = newScore.GetComponentsInChildren<TMP_Text>(); // Get components in children Name and score

            txts[0].text = score.Key; // First one is name
            txts[1].text = score.Value.ToString(); // Second one is the score

            newScore.GetComponentInChildren<UnityEngine.UI.Image>().color = new Color(Random.Range(0, 10) / 10, Random.Range(0, 10) / 10, Random.Range(0, 10) / 10, 1);
            spawnedHighscore.Add(newScore);
        }
    }
    void Sort(GameObject[] unsortedList)
    {
        int min;
        GameObject temp;
        int tempInt;
        Vector3 tempPos;

        int[] score = new int[unsortedList.Length];
        for (int i = 0; i < unsortedList.Length; i++)
        {
            string txt = unsortedList[i].GetComponent<TMP_Text>().text;
            int.TryParse(txt, out int tmp);
            score[i] = tmp;
        }

        for (int i = 0; i < unsortedList.Length; i++)
        {
            min = i;
            for (int j = i + 1; j < unsortedList.Length; j++)
            {
                if (score[j] < score[min])
                {
                    min = j;
                }
            }

            if (min != i)
            {
                temp = unsortedList[i];
                tempInt = score[i];

                unsortedList[i] = unsortedList[min];
                score[i] = score[min];

                unsortedList[min] = temp;
                score[min] = tempInt;

                tempPos = unsortedList[i].transform.localPosition;

                unsortedList[i].transform.localPosition = new Vector3(unsortedList[min].transform.localPosition.x, tempPos.y, tempPos.z);

                unsortedList[min].transform.localPosition = new Vector3(tempPos.x, unsortedList[min].transform.localPosition.y, unsortedList[min].transform.localPosition.z);
            }
        }
    }
    public void SaveLoadHigscore(bool save, string path = "")
    {
        string savecontent = "";
        if (save)
        {
            foreach (var item in highscore)
            {
                savecontent += item.Key + ":" + item.Value + ",";
            }
            File.WriteAllText(path, savecontent);
        }
        else
        {
            try
            {
                highscore = new Dictionary<string, int>(); // Reset 

                savecontent = File.ReadAllText(path); // Get the save data

                string[] highScores = savecontent.Split(','); // Split at each highscore
                for (int i = 0; i < highScores.Length - 1; i++) // Go throug all highscores and ad them to dictionary
                {
                    string[] scoreTbl = highScores[i].Split(':'); // Seperate key from value
                    int tmp = 0; // If corrpt file reset to 0;
                    int.TryParse(scoreTbl[1], out tmp);
                    highscore.Add(scoreTbl[0], tmp);
                }
            }
            catch (System.Exception)
            {
                Debug.Log("No save file.");
            }
        }
    }
}
