using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{

    [SerializeField] static GameObject pointTxt;

    private static int _score;
    Dictionary<string, int> highscore;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void Getpoints(int points,Vector3 location) // Give points to player
    {
        _score += points; // Add points to score
      GameObject newText=  Instantiate(pointTxt, location, Quaternion.identity); // Spawn the text
        newText.GetComponent<TMP_Text>().text = points.ToString(); // Show flash text
        newText.GetComponent<Animator>().Play(); // Start animation

    }
}
