using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour  //Emma. Fiendernas kod
                                    //Viktigt!!! Ifall ni anv�nder arv!!! F�r att n�got ska h�nda n�r fienderna r�r n�got m�ste man ha "OnCollisionEnter..."
                                    //Som finns l�ngst ner i koden, samt if-satsen med "collision.tag...". (Om man inte kan f� in de i en funktion p� n�got s�tt...?)
{
    public float hp;
    [SerializeField] private float moveSpeed;
    /*
    //Hur mycket den r�r sig i X-led (�t sidan). Det h�r �r h�lfte av v�rdet d� den r�r sig 2 g�nger innan den v�nder.
    [SerializeField] protected float moveSideSpeed = 1.5f;
    [SerializeField] protected float moveFrequency =1f;

    //Hur mycket den r�r sig i Y-led (ner). Det h�r �r h�lften av v�rdet d� den r�r sig 2 g�nger innan den v�nder.
    [SerializeField] protected float down = -0.25f;
    */
    [Tooltip("How low does the enemy go before it dies ")]
    [SerializeField] protected float minYpos;

    /*
    //Original positionens x-v�rde (�r n�r den �r l�ngst till v�nster)
    [Tooltip("Left most point on the x axis")]
    [SerializeField] protected float maxLeftPosX;
    [Tooltip("Right most point on the x axis")]
    [SerializeField] protected float maxRightPosX;
    */

    //[SerializeField] protected float timer = 0;
    [SerializeField] protected Vector2 gainPoints;
    [Tooltip("Shake intensity and duration")]
    [SerializeField] protected Vector2 shake;
    [SerializeField] ParticleSystem changeDirectionParticles;
    [SerializeField] ParticleSystem deathParticles;

    protected Rigidbody2D rb;

    public bool GivePoints;

    //Score score;

    //Camera cam;

    //CameraEffects cameraEffects;

    //Det h�r �r f�r att de ska "blinka" n�r de r�r sig.
    //private Color color;
    //[SerializeField] SpriteRenderer rend;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //cam = Camera.main;

        /*
        //F�rg saker
        rend = rend != null ? rend : GetComponentInChildren<SpriteRenderer>();

        color = rend.color;

        color.a = 1;
        rend.color = color;
        */

        //S�tter originalPosX
        // maxLeftPosX = transform.position.x;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (transform.position.y <= minYpos) // Vi tog oss genom spelarens niv�
        {
            HitPlayer(); // Skada spelaren
        }

        //Debug.Log(cam);

        //�r funktion f�r ifall man ska g�ra arv
        //Move();

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

    /*
    //Funktion med r�relse kod.
    protected virtual void Move()
    {
        timer += Time.deltaTime;

        //Fienden blir osynlig
        if (timer >= moveFrequency / 2 - 0.1f && timer <= moveFrequency / 2 + 0.1f) 
        {
            color.a = 0;
            rend.color = color;
        }
        //Fienden r�r sig
        else if (timer >= moveFrequency)
        {
            transform.position += new Vector3(moveSideSpeed, 0, 0); // G� �t sidan

            //Fienden poppar fram igen (�r i samma som att g� f�r att g� saken endast ska h�nda en g�ng)
            color.a = 1;
            rend.color = color;

            timer = 0;
        }

        //Ifall den �r vid extrem punkterna skall den v�nda om och hoppa ner. // Tv� "steg" fr�n original positionen, eller i origianl positionen (b�da �r ett "steg" fr�n mitten)
        if (transform.position.x <= maxLeftPosX && moveSideSpeed < 0 || transform.position.x >= maxRightPosX && moveSideSpeed > 0)
        {
            transform.position += new Vector3(0, -down, 0); // Ramla en rad ner
            //Byter h�ll
            moveSideSpeed *= -1;
            changeDirectionParticles.Play();
        }

        if (transform.position.y <= minYpos) // Vi tog oss genom spelarens niv�
        {
            HitPlayer(); // Skada spelaren
        }
    }
    */

    protected virtual void FixedUpdate()
    {
        rb.velocity = Vector2.down * moveSpeed;
    }

    //Det h�r h�nder n�r fienderna r�r n�got som inte �r spelaren. (Aktiveras i "OnCollisionEnter...").
    public virtual void Die(bool givePoints)
    {
        //Kopierat fr�n Score-script. Vet inte hur po�ngen funkar, s� ifall det h�r �r fel �r det bara att �ndra.
        //Just nu kommer det ett error meddelande, som s�ger att det �r fel p� Score 63, men vet inte om det �r pga den h�r koden
        //Score koden, eller ifall det �r f�r att n�got saknas???
        //Vector3 mousePos = Input.mousePosition;
        //mousePos.z = -cam.transform.position.z;

        if (GivePoints && givePoints)
        {
            Score.AddPoints(Mathf.RoundToInt(Random.Range(gainPoints.x, gainPoints.y)), transform.position); // Give points
        }

        //funkar inte (Vet inte om det �r f�r att min test kamera saknar Flash image?).
        CameraEffects.Shake(shake.x, shake.y);

        deathParticles.Play(); // Play death particles
        deathParticles.transform.parent = null; // Deatach from parent

        Destroy(deathParticles.gameObject, 2); //Destroy them later
        Destroy(gameObject); // F�st�r objectet efter att vi har get po�ng

    }

    public virtual void Hurt(float damage)
    {
        hp -= damage;
        //DPSCounter.Add(damage); // Visa v�r damage per second
        if (hp <= 0)
        {
            Die(true);
        }
    }

    public virtual void HitPlayer() // Enemy hit player or player snuck past player
    {
        Die(false);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            //Spelaren f�rlorar liv/spelet?
            HitPlayer();
        }
        //Bullet koderna verkar ha n�got f�r det h�r redan?
        //S� l�nge den inte r�r kanten borde det vara okej, men ifall vi l�gger en tag p� skotten borde vi �ndra den h�r
        //f�r s�kerhets skull
        /*else if (collision.transform.CompareTag("Bullet")) // Coliderar med ett skot
        {
            collision.TryGetComponent<Bullet>(out Bullet bullet);
            Hurt(bullet.Damage); // Skada oss
        }*/
    }
}