using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;


public class Score : MonoBehaviour
{
    public static Score Instance;

    public static int PlayerScore = 0;

    [SerializeField] GameObject pointTxt;
    [SerializeField] TMP_Text scoreText;

    Camera cam;
    Animator expand;
    int _oldScore = 0;

    Color scoreColor = new Color(1, 1, 1, 1); // The color of the scor text

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        expand = scoreText.GetComponent<Animator>(); // Get animator from score text
        cam = Camera.main; // Set the camera
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKey(KeyCode.Space)) // Debug
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -cam.transform.position.z;
            AddPointsLocal(Random.Range(20, 100), cam.ScreenToWorldPoint(mousePos));
        }
        */
        
        if (PlayerScore != _oldScore) // Update score
        {
            _oldScore = PlayerScore; // Uppdate old score
            scoreText.color = scoreColor;
            scoreText.text = "Score: " + PlayerScore; // Write message

            expand.SetTrigger("Expand"); // Expand and retract
        }

    }

    /// <summary>
    /// Spawn a text objet at the position with the amount specified.
    /// </summary>
    /// <param name="points">Amount of points to spawn in world.</param>
    /// <param name="location">Location to spawn at in WORLD SPACE</param>
    public static void AddPoints(int points, Vector3 location)
    {
        Instance.AddPointsLocal(points, location);
    }

    private void AddPointsLocal(int points, Vector3 location) // Give points to player
    {
        PlayerScore += points; // Add points to score
        GameObject newText = Instantiate(pointTxt, cam.WorldToScreenPoint(location), Quaternion.identity, transform); // Spawn the text
        newText.SetActive(true);
        TMP_Text tMP_Text = newText.GetComponentInChildren<TMP_Text>();

        tMP_Text.text = points.ToString(); // Show points gained

        /*
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
        */

        //newText.GetComponentInChildren<Animator>().Play("TextFlash"); // Start animation
        Destroy(newText, 3); // Destroy the exes clones after some time     
    }
}