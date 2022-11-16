using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// 
/// </summary>
public class DamageDisplay : MonoBehaviour
{
    public static DamageDisplay Instance;

    [SerializeField] GameObject pointTxt;
    Camera cam;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main; // Set the camera
    }

    /// <summary>
    /// Spawn a text objet at the position displaying the amount specified.
    /// </summary>
    /// <param name="damage">Amount of damage to spawn in world.</param>
    /// <param name="location">Location to spawn at in WORLD SPACE</param>
    public static void DisplayDamage(float damage, Vector3 location)
    {
        Instance.DisplayDamageLocal(damage, location);
    }

    private void DisplayDamageLocal(float damage, Vector3 location) // Give points to player
    {
        GameObject newText = Instantiate(pointTxt, cam.WorldToScreenPoint(location), Quaternion.Euler(0, 0, Random.Range(-25, 25)), transform); // Spawn the text
        newText.SetActive(true);

        newText.GetComponentInChildren<TMP_Text>().text = damage.ToString(); // Show damage done gained

        //newText.GetComponentInChildren<Animator>().Play("TextFlash"); // Start animation
        Destroy(newText, 3); // Destroy the exes clones after some time     
    }
}
