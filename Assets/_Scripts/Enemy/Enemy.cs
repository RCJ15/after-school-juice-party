using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour  //Emma
{
    //Hur mycket den r�r sig i X-led (�t sidan). Det h�r �r h�lfte av v�rdet d� den r�r sig 2 g�nger.
    private float side = 2f;

    //Hur mycket den r�r sig i Y-led (ner). Det h�r �r h�lften av v�rdet d� den r�r sig 2 g�nger.
    private float down = -0.25f;

    //Original positionens x-v�rde (�r n�r den �r l�ngst till v�nster)
    private float originalPosX;

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
        //Fienden r�r sig
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

        //Ifall den �r tv� "steg" fr�n original positionen, eller i origianl positionen (b�da �r ett "steg" fr�n mitten)
        if (transform.position.x == originalPosX + 2*side && side > 0 || transform.position.x == originalPosX && side < 0)
        {
            //Byter h�ll
            side *= -1;
        }

        //Nere vid kanten.
        if (transform.position.x <= -5)
        {
            //F�rlora liv/spelet.
        }
    }
}
