using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using TMPro;

public class HighScore : MonoBehaviour
{
    public static readonly string SaveFolder = Application.streamingAssetsPath;
    public static readonly string SavePath = Path.Combine(SaveFolder, "SaveFile.txt");

    public static string PlayerName = "";

    [Header("Highscore")]
    [SerializeField] GameObject highscorePanel;
    [SerializeField] GameObject content;
    [SerializeField] GameObject playerScoreTemplate;
    [Space]
    [Header("Get player name")]
    [SerializeField] GameObject inputName;
    [SerializeField] TMP_InputField inputNameInputField;
    [SerializeField] TMP_Text inputScoreTMPText;
    [SerializeField] UnityEngine.UI.Button confirmPlayerNameButton;


    Dictionary<string, int> highscore = new Dictionary<string, int>();
    List<GameObject> spawnedHighscore = new List<GameObject>();
    void Start()
    {
        Load();
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
        if (Input.GetKeyDown(KeyCode.F1)) // Debug
        {
            Save();
        }
    }

    /// <summary>
    /// Restart level
    /// </summary>
    public void Retry()
    {

    }
    /// <summary>
    /// Go back to the main menu
    /// </summary>
    public void MainMenu()
    {

    }

    /*
    /// <summary>
    /// Idk what yet :/
    /// </summary>
    public void LastButton()
    {

    }
    */

    /// <summary>
    /// Resets the players name and score
    /// </summary>
    void ResetScore()
    {
        Score.PlayerScore = 0;
        PlayerName = "";
    }

    /// <summary>
    /// Asign players name and add to score board
    /// </summary>
    /// <param name="confirm">Confirm the players name</param>
    public void GetPlayerName(bool confirm)
    {
        inputScoreTMPText.text = Score.PlayerScore.ToString();

        if (confirm && PlayerName == "") // Set players name
        {
            PlayerName = inputNameInputField.text; // Get name from inputfield
            if (PlayerName == "" || PlayerName==null) // Still not set, player left it blank
            {
                PlayerName = "Player" + Score.PlayerScore; // Assign default name
            }

            inputName.SetActive(false); // Hide panel
            AddToHighscore(PlayerName, Score.PlayerScore);
            Sort();
            PrintHighScore();

            Save();
        }
        else if (PlayerName == "") // Player has no name yet
        {
            inputName.SetActive(true);

            /*
            Animator[] animators = inputName.GetComponentsInChildren<Animator>();

            animators[0].Play("Wobbel", 0);
            animators[1].Play("Swing", 0);
            animators[2].Play("InputShake", 0);
            */
        }
    }

    /// <summary>
    /// Add a entry to highscore.
    /// </summary>
    /// <param name="playerName">Name of player.</param>
    /// <param name="score">Score of player.</param>
    public void AddToHighscore(string playerName, int score)
    {
        playerName = playerName.Trim();

        highscore[playerName] = score; // Add entry to list
    }
    /// <summary>
    /// Show the highscore panel
    /// </summary>
   public void ShowHighScoreTable()
    {
        if (highscorePanel.activeSelf)
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
    /*
    // We should instead use the Linq sort that is built in instead of making a custom sort algorithm
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
    */

    /// <summary>
    /// Ensure no ':' and ',' are in the player name input field
    /// </summary>
    /// <param name="playerName"></param>
    public void FormatPlayerName(string playerName)
    {
        string newString = "";

        for (int i = 0; i < playerName.Length; i++)
        {
            char c = playerName[i];

            if (c == ':' || c == ',')
            {
                continue;
            }

            newString += c;
        }

        inputNameInputField.text = newString;
    }

    public void UpdateConfirmButton(string playerName)
    {
        confirmPlayerNameButton.interactable = !string.IsNullOrEmpty(playerName);
    }

    /// <summary>
    /// Saves all highscores to file
    /// </summary>
    public void Save()
    {
        string content = "";

        foreach (var item in highscore)
        {
            // Do corret format
            content += item.Key + ":" + item.Value + ",";
        }

        if (!Directory.Exists(SaveFolder)) // Create directory
        {
            Directory.CreateDirectory(SaveFolder);
        }

#if UNITY_EDITOR
        Debug.Log("Saved!");
#endif

        File.WriteAllText(SavePath, content);
    }

    /// <summary>
    /// Loads all highscores from file
    /// </summary>
    public void Load()
    {
        highscore = new Dictionary<string, int>(); // Reset 

        if (!Directory.Exists(SaveFolder)) // Create directory
        {
            Directory.CreateDirectory(SaveFolder);
        }

        // Check if file doesn't exist
        if (!File.Exists(SavePath))
        {
#if UNITY_EDITOR
            Debug.Log("No save file.");
#endif

            // Can't load
            return;
        }

        string content = File.ReadAllText(SavePath); // Read save data

        if (string.IsNullOrEmpty(content)) // Check if file is empty
        {
            // Can't load
            return;
        }

        string[] highScores = content.Split(','); // Split at each highscore

        if (highScores.Length <= 0) // Check if we have any highscores
        {
            // Can't load
            return;
        }

        for (int i = 0; i < highScores.Length; i++) // Go throug all highscores and ad them to dictionary
        {
            string[] scoreTbl = highScores[i].Split(':'); // Seperate key from value

            if (scoreTbl.Length != 2) // Must be length 2 otherwise it's a corrupt highscore
            {
                continue;
            }

            int.TryParse(scoreTbl[1], out int tmp); // Doig "out int tmp" will create a new int variable
            highscore.Add(scoreTbl[0], tmp);
        }

        // Sort highscores by highest score at top
        Sort();
    }

    /// <summary>
    /// Will sort all highscores from highest to lowest
    /// </summary>
    public void Sort()
    {
        // Linq is confusig
        var list = highscore.OrderByDescending(pair => pair.Value);

        highscore = new Dictionary<string, int>();
        foreach (var pair in list)
        {
            highscore.Add(pair.Key, pair.Value);
        }
    }
}
