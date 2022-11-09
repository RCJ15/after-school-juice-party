using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour  //Emma. Fiendernas kod
    //Viktigt!!! Ifall ni använder arv!!! För att något ska hända när fienderna rör något måste man ha "OnCollisionEnter..."
    //Som finns längst ner i koden, samt if-satsen med "collision.tag...". (Om man inte kan få in de i en funktion på något sätt...?)
{
    //Hur mycket den rör sig i X-led (åt sidan). Det här är hälfte av värdet då den rör sig 2 gånger innan den vänder.
    private float side = 0.5f;

    //Hur mycket den rör sig i Y-led (ner). Det här är hälften av värdet då den rör sig 2 gånger innan den vänder.
    private float down = -0.25f;

    //Original positionens x-värde (är när den är längst till vänster)
    private float originalPosX;

    private float timer = 0;

    //Score score;

    //Camera cam;

    //CameraEffects cameraEffects;

    //Det här är för att de ska "blinka" när de rör sig.
    private Color color;
    private SpriteRenderer rend;

    // Start is called before the first frame update
    void Start()
    {
        //cam = Camera.main;

        //Färg saker
        rend = GetComponent<SpriteRenderer>();

        color = rend.color;

        color.a = 1;
        rend.color = color;

        //Sätter originalPosX
        originalPosX = transform.position.x;

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(cam);

        //Är funktion för ifall man ska göra arv
        Move();

        //Det här är temporärt, Debug.
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(gameObject);

            //Kopierat från Score-script. Vet inte hur poängen funkar, så ifall det här är fel är det bara att ändra.
            //Just nu kommer det ett error meddelande, som säger att det är fel på Score 63, men vet inte om det är pga den här koden
            //Score koden, eller ifall det är för att något saknas???
            //Vector3 mousePos = Input.mousePosition;
            //mousePos.z = -cam.transform.position.z;
            
            Score.AddPoints(1, transform.position);

            //Debug.Log(mousePos);
            //Debug.Log(cam);


            //funkar inte (Vet inte om det är för att min test kamera saknar Flash image?).
            CameraEffects.Shake(1, 1);
        }*/
    }

    //Funktion med rörelse kod.
    protected virtual void Move()
    {
        timer += Time.deltaTime;

        //Fienden blir osynlig
        if (timer >= 0.5f && timer <= 0.6f)
        {
            color.a = 0;
            rend.color = color;
        }
        //Fienden rör sig
        else if (timer >= 0.8f)
        {
            transform.position += new Vector3(side, down, 0);

            //Fienden poppar fram igen (är i samma som att gå för att gå saken endast ska hända en gång)
            color.a = 1;
            rend.color = color;

            timer = 0;
        }

        //Ifall den är två "steg" från original positionen, eller i origianl positionen (båda är ett "steg" från mitten)
        if (transform.position.x == originalPosX + 2 * side && side > 0 || transform.position.x == originalPosX && side < 0)
        {
            //Byter håll
            side *= -1;
        }

    }
    //Det här händer när fienderna rör något som inte är spelaren. (Aktiveras i "OnCollisionEnter...").
    protected virtual void EnemyDies()
    {
        Destroy(gameObject);

        //Kopierat från Score-script. Vet inte hur poängen funkar, så ifall det här är fel är det bara att ändra.
        //Just nu kommer det ett error meddelande, som säger att det är fel på Score 63, men vet inte om det är pga den här koden
        //Score koden, eller ifall det är för att något saknas???
        //Vector3 mousePos = Input.mousePosition;
        //mousePos.z = -cam.transform.position.z;

        // Gammal kod förösker spawna score på musen, detta gör så att score spawnas på detta object
        Score.AddPoints(1, transform.position);

        //funkar inte (Vet inte om det är för att min test kamera saknar Flash image?).
        CameraEffects.Shake(1, 1);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.transform.tag == "Player")
       {
            //Spelaren förlorar liv/spelet?
       }
       //Bullet koderna verkar ha något för det här redan?
       //Så länge den inte rör kanten borde det vara okej, men ifall vi lägger en tag på skotten borde vi ändra den här
       //för säkerhets skull
       else
       {
            EnemyDies();
       }
    }


}
