using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour  //Emma
{
    //Hur mycket den r�r sig i X-led (�t sidan). Det h�r �r h�lfte av v�rdet d� den r�r sig 2 g�nger.
    private float side = 2f;

    //Hur mycket den r�r sig i Y-led (ner). Det h�r �r h�lften av v�rdet d� den r�r sig 2 g�nger.
    private float down = -0.25f;

    //N�r det �r dags f�r dem att v�nda
    private float turn = 0;

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
            Debug.Log(side);

            Debug.Log(transform.position);

            transform.position += new Vector3(side, down, 0);

            Debug.Log(transform.position);

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

        Debug.Log(side);
        Debug.Log(turn);
        
        /*
        if (timer >= 0.8f && timer <= 0.9f)
        {
            color.a = 0;
            rend.color = color;
        }
        //Fienden r�r sig
        else if (timer >= 1 && timer <= 1.1f)
        {
            Debug.Log(side);

            Debug.Log(transform.position);

            transform.position += new Vector3(side, down, 0);

            Debug.Log(transform.position);

            //N�sta g�ng ska de r�ra sig �t andra h�llet (i x-led).
            side *= -1;
        }
        //Fienden poppar fram igen
        if (timer >= 1)
        {
            color.a = 1;
            rend.color = color;

            timer = 0;
        }
        //Resultat = ser ut som om fienden blinkar n�r den r�r sig (/"teleporterar" sig fram).
        /*
        else if (timer >= 0.65f && timer < 0.8f)
        {
            color.a = 1;
            rend.color = color;
        }
        else if (timer >= 0.8f && timer < 0.95f)
        {
            color.a = 0;
            rend.color = color;
        }*/
    }
}
