using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FelEnemy : MonoBehaviour  //Emma
    //Det h�r scriptet �r antagligen fel. Har sparat det ifall jag har fel (och det �r r�tt script).
    //Det �r inte heller klart, s� ifall den h�r beh�vs m�ste vi g�ra klart den. Se "Enemy" f�r den nya koden.
{
    private float speed = 2f;

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
        if(timer >= 2.5f && timer <= 2.7f)
        {              

            Debug.Log(rend.color);

            transform.position += new Vector3(speed, 0, 0);
        }
        //Fienden poppar fram igen
        if (timer >= 2.5f)
        {       
            color.a = 1;
            rend.color = color;

            timer = 0;
        }
        //Resultat = ser ut som om fienden blinkar n�r den r�r sig (/"teleporterar" sig fram).

        //Funkar 
        if (transform.position.x == 9.79f)
        {
            Debug.Log("123");

            speed *= -1;
        }
    }
}
