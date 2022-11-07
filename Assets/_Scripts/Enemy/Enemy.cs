using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour  //Emma. Fiendernas kod
{
    //Hur mycket den r�r sig i X-led (�t sidan). Det h�r �r h�lfte av v�rdet d� den r�r sig 2 g�nger innan den v�nder.
    private float side = 2f;

    //Hur mycket den r�r sig i Y-led (ner). Det h�r �r h�lften av v�rdet d� den r�r sig 2 g�nger innan den v�nder.
    private float down = -0.25f;

    //Original positionens x-v�rde (�r n�r den �r l�ngst till v�nster)
    private float originalPosX;

    private float timer = 0;

    Score score;

    [SerializeField]
    Camera cam;

    CameraEffects cameraEffects;

    //Det h�r �r f�r att de ska "blinka" n�r de r�r sig.
    private Color color;
    private SpriteRenderer rend;

    // Start is called before the first frame update
    void Start()
    {
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
        //�r funktion f�r ifall man ska g�ra arv
        Move();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(gameObject);

            //Kopierat fr�n Score-script. Vet inte hur po�ngen funkar, s� ifall det h�r �r fel �r det bara att �ndra.
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -cam.transform.position.z;
            Score.AddPoints(1, cam.ScreenToWorldPoint(mousePos));

            Debug.Log(mousePos);
            Debug.Log(cam);


            //funkar inte
            CameraEffects.Shake(1, 1);
        }
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
           Destroy(gameObject);

           //Kopierat fr�n Score-script. Vet inte hur po�ngen funkar, s� ifall det h�r �r fel �r det bara att �ndra.
           Vector3 mousePos = Input.mousePosition;
           mousePos.z = -cam.transform.position.z;
           Score.AddPoints(1, cam.ScreenToWorldPoint(mousePos));

           //funkar ???
           CameraEffects.Shake(1, 1);
       }
    }


}
