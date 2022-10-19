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
    public static int playerScore = 0;
    int _oldScore = -1;

    Color scoreColor = new Color(1, 1, 1, 1); // The color of the scor text


    // Start is called before the first frame update
    void Start()
    {
        expand = scoreText.GetComponent<Animator>(); // Get animator from score text
        cam = Camera.main; // Set the camera
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            //Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            //mousePos.z = 0;
            Getpoints(Random.Range(20, 100), Input.mousePosition);
        }
        

        if (playerScore != _oldScore) // Update score
        {
            _oldScore = playerScore; // Uppdate old score
            scoreText.color = scoreColor;
            scoreText.text = "Score: " + playerScore; // Write message

            expand.Play(0); // Expand and retract
        }

    }

    /// <summary>
    /// Spawn a text objet at the position with the amount specified.
    /// </summary>
    /// <param name="points">Amount of points to spawn in world.</param>
    /// <param name="location">Location to spawn at.</param>
    public void Getpoints(int points, Vector3 location) // Give points to player
    {
        playerScore += points; // Add points to score
        GameObject newText = Instantiate(pointTxt, location, Quaternion.identity, transform); // Spawn the text
        newText.SetActive(true);
        TMP_Text tMP_Text = newText.GetComponentInChildren<TMP_Text>();

        tMP_Text.text = "+" + points.ToString(); // Show points gained

        if (points < 25) // A¨ssign a cool color
        {
            scoreColor = new Color(.5f, 0.5f, 0, 1); // Make the score have the same color as the last point gained
            tMP_Text.CrossFadeColor(scoreColor, .3f, false, false); // Sett the color
        }
        else if (points > 25 && points < 50)
        {
            scoreColor = new Color(0, 0.5f, 0.8f, 1);
            tMP_Text.CrossFadeColor(scoreColor, .3f, false, false);
        }
        else
        {
            scoreColor = new Color(0.8f, 0.5f, 0, 1);
            tMP_Text.CrossFadeColor(scoreColor, .3f, false, false);
        }

        newText.GetComponentInChildren<Animator>().Play("TextFlash"); // Start animation
        Destroy(newText, 3); // Destroy the exes clones after some time     
    }




}