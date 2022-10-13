using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;


public class Score : MonoBehaviour
{

    [SerializeField] GameObject pointTxt;
    [SerializeField] TMP_Text scoreText;

    Camera cam;
    Animator expand;
    private int _score = 0;
    private int _oldScore = -1;
    Dictionary<string, int> highscore = new Dictionary<string, int>();


    // Start is called before the first frame update
    void Start()
    {
        expand = scoreText.GetComponent<Animator>(); // Get animator from score text
        cam = Camera.main;
        Debug.Log(cam);
        Debug.Log(Camera.main) ;


        highscore.Add("aha", 124124);
        highscore.Add("gtt", 4567554);
        highscore.Add("ynhnh", 957);
        highscore.Add("gtmhnlkt", 53487);
        highscore.Add("hdhn", 3454750);

        SaveLoadHigscore(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Getpoints(Random.Range(20, 100), Input.mousePosition);
        }

        if (_score != _oldScore) // Update score
        {
            _oldScore = _score; // Uppdate old score
            scoreText.text = "Score: " + _score; // Write message

            expand.Play(0); // Expand and retract
        }

    }
    public void Getpoints(int points, Vector3 location) // Give points to player
    {
        _score += points; // Add points to score
        GameObject newText = Instantiate(pointTxt, location, Quaternion.identity, transform); // Spawn the text
        newText.SetActive(true);
        TMP_Text tMP_Text = newText.GetComponent<TMP_Text>();

        tMP_Text.text = "+" + points.ToString(); // Show points gained

        if (points < 25) // asign a cool color
        {
            tMP_Text.CrossFadeColor(new Color(.5f, 0.5f, 0, 1), .3f, false, false);
        }
        else if (points > 25 && points < 50)
        {
            tMP_Text.CrossFadeColor(new Color(0, 0.5f, 0.8f, 1), .3f, false, false);
        }
        else
        {
            tMP_Text.CrossFadeColor(new Color(0.8f, 0.5f, 0, 1), .3f, false, false);
        }

        newText.GetComponent<Animator>().Play("TextFlash"); // Start animation
        Destroy(newText, 3); // Destroy the exes clones        
    }

    public void AddToHighscore(string playerName, int score)
    {
        highscore.Add(playerName, score);
    }
    void Sort<T>(T list)
    {

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