using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour  //Emma. Fiendernas kod
    //Viktigt!!! Ifall ni anv�nder arv!!! F�r att n�got ska h�nda n�r fienderna r�r n�got m�ste man ha "OnCollisionEnter..."
    //Som finns l�ngst ner i koden, samt if-satsen med "collision.tag...". (Om man inte kan f� in de i en funktion p� n�got s�tt...?)
{
    //Hur mycket den r�r sig i X-led (�t sidan). Det h�r �r h�lfte av v�rdet d� den r�r sig 2 g�nger innan den v�nder.
    private float side = 0.5f;

    //Hur mycket den r�r sig i Y-led (ner). Det h�r �r h�lften av v�rdet d� den r�r sig 2 g�nger innan den v�nder.
    private float down = -0.25f;

    //Original positionens x-v�rde (�r n�r den �r l�ngst till v�nster)
    private float originalPosX;

    private float timer = 0;

    //Score score;

    //Camera cam;

    //CameraEffects cameraEffects;

    //Det h�r �r f�r att de ska "blinka" n�r de r�r sig.
    private Color color;
    private SpriteRenderer rend;

    // Start is called before the first frame update
    void Start()
    {
        //cam = Camera.main;

        //F�rg saker
        rend = GetComponent<SpriteRenderer>();

        color = rend.color;

        color.a = 1;
        rend.color = color;

        //S�tter originalPosX
        originalPosX = transform.position.x;

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(cam);

        //�r funktion f�r ifall man ska g�ra arv
        Move();

        //Det h�r �r tempor�rt, Debug.
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(gameObject);

            //Kopierat fr�n Score-script. Vet inte hur po�ngen funkar, s� ifall det h�r �r fel �r det bara att �ndra.
            //Just nu kommer det ett error meddelande, som s�ger att det �r fel p� Score 63, men vet inte om det �r pga den h�r koden
            //Score koden, eller ifall det �r f�r att n�got saknas???
            //Vector3 mousePos = Input.mousePosition;
            //mousePos.z = -cam.transform.position.z;
            
            Score.AddPoints(1, transform.position);

            //Debug.Log(mousePos);
            //Debug.Log(cam);


            //funkar inte (Vet inte om det �r f�r att min test kamera saknar Flash image?).
            CameraEffects.Shake(1, 1);
        }*/
    }

    //Funktion med r�relse kod.
    protected virtual void Move()
    {
        timer += Time.deltaTime;

        //Fienden blir osynlig
        if (timer >= 0.5f && timer <= 0.6f)
        {
            color.a = 0;
            rend.color = color;
        }
        //Fienden r�r sig
        else if (timer >= 0.8f)
        {
            transform.position += new Vector3(side, down, 0);

            //Fienden poppar fram igen (�r i samma som att g� f�r att g� saken endast ska h�nda en g�ng)
            color.a = 1;
            rend.color = color;

            timer = 0;
        }

        //Ifall den �r tv� "steg" fr�n original positionen, eller i origianl positionen (b�da �r ett "steg" fr�n mitten)
        if (transform.position.x == originalPosX + 2 * side && side > 0 || transform.position.x == originalPosX && side < 0)
        {
            //Byter h�ll
            side *= -1;
        }

    }
    //Det h�r h�nder n�r fienderna r�r n�got som inte �r spelaren. (Aktiveras i "OnCollisionEnter...").
    protected virtual void EnemyDies()
    {
        Destroy(gameObject);

        //Kopierat fr�n Score-script. Vet inte hur po�ngen funkar, s� ifall det h�r �r fel �r det bara att �ndra.
        //Just nu kommer det ett error meddelande, som s�ger att det �r fel p� Score 63, men vet inte om det �r pga den h�r koden
        //Score koden, eller ifall det �r f�r att n�got saknas???
        //Vector3 mousePos = Input.mousePosition;
        //mousePos.z = -cam.transform.position.z;

        // Gammal kod f�r�sker spawna score p� musen, detta g�r s� att score spawnas p� detta object
        Score.AddPoints(1, transform.position);

        //funkar inte (Vet inte om det �r f�r att min test kamera saknar Flash image?).
        CameraEffects.Shake(1, 1);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.transform.tag == "Player")
       {
            //Spelaren f�rlorar liv/spelet?
       }
       //Bullet koderna verkar ha n�got f�r det h�r redan?
       //S� l�nge den inte r�r kanten borde det vara okej, men ifall vi l�gger en tag p� skotten borde vi �ndra den h�r
       //f�r s�kerhets skull
       else
       {
            EnemyDies();
       }
    }


}
