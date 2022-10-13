using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;


public class Score : MonoBehaviour
{

    [SerializeField] GameObject pointTxt;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] GameObject content;
    [SerializeField] GameObject playerScoreTemplate;
    [SerializeField] GameObject highscorePanel;

    Camera cam;
    Animator expand;
    private int _score = 0;
    private int _oldScore = -1;
    Dictionary<string, int> highscore = new Dictionary<string, int>();
    List<GameObject> spawnedHighscore = new List<GameObject>();

    Color scoreColor =new Color(1, 1, 1, 1);


    // Start is called before the first frame update
    void Start()
    {
        expand = scoreText.GetComponent<Animator>(); // Get animator from score text
        cam = Camera.main;

        AddToHighscore("Anna", 89737634);
        AddToHighscore("Emma", 843956487);
        AddToHighscore("Ruben", 98765);
        AddToHighscore("Rubem", 6237384);
        AddToHighscore("eg,jhehgjrbejhbvejbhjvbjhvbjheb", 93);
        AddToHighscore("Gejfhgvegujgefhjekjgkehgkhehgieuhgisehbiehsihbresgrga", 939859833);
        AddToHighscore("Grkjeshkjgsssssssssssssssskkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkga", 93);
        AddToHighscore("Gejjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjrga", 93);
        AddToHighscore("Geskjfgsiegiuuuuuuuuuuuuuuuuuuuuuuuurga", 999999999);
        AddToHighscore("Grga", 938592893);
        AddToHighscore("Gregory", 9223);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            Getpoints(Random.Range(20, 100), mousePos);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            ShowHighScoreTable();
        }

        if (_score != _oldScore) // Update score
        {
            _oldScore = _score; // Uppdate old score
            scoreText.color = scoreColor;
            scoreText.text = "Score: " + _score; // Write message

            expand.Play(0); // Expand and retract
        }

    }
    public void Getpoints(int points, Vector3 location) // Give points to player
    {
        _score += points; // Add points to score
        GameObject newText = Instantiate(pointTxt, location, Quaternion.identity, transform); // Spawn the text
        newText.SetActive(true);
        TMP_Text tMP_Text = newText.GetComponentInChildren<TMP_Text>();

        tMP_Text.text = "+" + points.ToString(); // Show points gained

        if (points < 25) // asign a cool color
        {
            scoreColor = new Color(.5f, 0.5f, 0, 1);
            tMP_Text.CrossFadeColor(scoreColor, .3f, false, false);
        }
        else if (points > 25 && points < 50)
        {scoreColor= new Color(0, 0.5f, 0.8f, 1);
            tMP_Text.CrossFadeColor(scoreColor, .3f, false, false);
        }
        else
        {
            scoreColor = new Color(0.8f, 0.5f, 0, 1);
            tMP_Text.CrossFadeColor(scoreColor, .3f, false, false);
        }

        newText.GetComponentInChildren<Animator>().Play("TextFlash"); // Start animation
        Destroy(newText, 3); // Destroy the exes clones        
    }

    public void AddToHighscore(string playerName, int score)
    {
        highscore.Add(playerName, score);
    }

    void ShowHighScoreTable()
    {
        if (highscorePanel.activeInHierarchy)
        {
            highscorePanel.SetActive(false);
        }
        else
        {
            highscorePanel.SetActive(true);
            PrintHighScore();
        }
    }
    void PrintHighScore()
    {
        foreach (var item in spawnedHighscore)
        {
            Destroy(item);
        }
        spawnedHighscore = new List<GameObject>();
        foreach (var score in highscore) // Add every highscore
        {
            GameObject newScore = Instantiate(playerScoreTemplate, content.transform); // Add new section
            newScore.SetActive(true);
            TMP_Text[] txts = newScore.GetComponentsInChildren<TMP_Text>(); // Get components in children Name and score

            txts[0].text = score.Key; // First one is name
            txts[1].text = score.Value.ToString(); // Second one is the score

            newScore.GetComponentInChildren<UnityEngine.UI.Image>().color = new Color(Random.Range(0, 10) / 10, Random.Range(0, 10) / 10, Random.Range(0, 10) / 10,1);
            spawnedHighscore.Add(newScore);
        }
    }
    void Sort(GameObject[]unsortedList)
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
            for (int j =i+1; j < unsortedList.Length; j++)
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