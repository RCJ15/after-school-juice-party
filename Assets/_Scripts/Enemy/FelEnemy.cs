using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FelEnemy : MonoBehaviour  //Emma
    //Det här scriptet är antagligen fel. Har sparat det ifall jag har fel (och det är rätt script).
    //Det är inte heller klart, så ifall den här behövs måste vi göra klart den. Se "Enemy" för den nya koden.
{
    private float speed = 2f;

    private float timer = 0;

    //Det här är för att de ska "blinka" när de rör sig.
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
        //Fienden rör sig
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
        //Resultat = ser ut som om fienden blinkar när den rör sig (/"teleporterar" sig fram).

        //Funkar 
        if (transform.position.x == 9.79f)
        {
            Debug.Log("123");

            speed *= -1;
        }
    }
}
