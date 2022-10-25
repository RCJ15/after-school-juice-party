using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour  //Emma
{
    //Hur mycket den rör sig i X-led (åt sidan). Det här är hälfte av värdet då den rör sig 2 gånger.
    private float side = 2f;

    //Hur mycket den rör sig i Y-led (ner). Det här är hälften av värdet då den rör sig 2 gånger.
    private float down = -0.25f;

    //Original positionens x-värde (är när den är längst till vänster)
    private float originalPosX;

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

        originalPosX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;

        //Fienden blir osynlig
        if (timer >= 0.5f && timer <= 0.6f)
        {
            color.a = 0;
            rend.color = color;
        }
        //Fienden rör sig
        else if (timer >= 0.8f && timer <= 0.85f)
        {
            transform.position += new Vector3(side, down, 0);
        }
        //Fienden poppar fram igen
        if (timer >= 0.8f)
        {
            color.a = 1;
            rend.color = color;

            timer = 0;
        }

        //Ifall den är två "steg" från original positionen, eller i origianl positionen (båda är ett "steg" från mitten)
        if (transform.position.x == originalPosX + 2*side && side > 0 || transform.position.x == originalPosX && side < 0)
        {
            //Byter håll
            side *= -1;
        }

        //Nere vid kanten.
        if (transform.position.x <= -5)
        {
            //Förlora liv/spelet.
        }
    }
}
