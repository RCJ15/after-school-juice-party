using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour  //Emma
{
    //X-led
    private float side = 3f;

    //Y-led
    private float down = -0.5f;

    private float timer = 0;

    //Det h�r �r f�r att de ska "blinka" n�r de r�r sig.
    private Color color;

    private SpriteRenderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();

        color = rend.color;

        color.a = 1;
        rend.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        //Fienden blir osynlig
        if (timer >= 2 && timer <= 2.2f)
        {
            color.a = 0;
            rend.color = color;
        }
        //Fienden r�r sig
        if (timer >= 2.5f && timer <= 2.7f)
        {

            Debug.Log(side);

            Debug.Log(transform.position);

            transform.position += new Vector3(side, down, 0);

            Debug.Log(transform.position);

            //N�sta g�ng ska de r�ra sig �t andra h�llet (x-led).
            side *= -1;
        }
        //Fienden poppar fram igen
        if (timer >= 2.5f)
        {
            color.a = 1;
            rend.color = color;

            timer = 0;
        }
        //Resultat = ser ut som om fienden blinkar n�r den r�r sig (/"teleporterar" sig fram).

    }
}
