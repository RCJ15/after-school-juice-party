using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour  //Emma. Fiendernas kod
                                    //Viktigt!!! Ifall ni anv�nder arv!!! F�r att n�got ska h�nda n�r fienderna r�r n�got m�ste man ha "OnCollisionEnter..."
                                    //Som finns l�ngst ner i koden, samt if-satsen med "collision.tag...". (Om man inte kan f� in de i en funktion p� n�got s�tt...?)
{
    public float hp;

    //Hur mycket den r�r sig i X-led (�t sidan). Det h�r �r h�lfte av v�rdet d� den r�r sig 2 g�nger innan den v�nder.
    [SerializeField] protected float moveLerp = 10f;
    [SerializeField] protected float moveSideSpeed = 1.5f;
    [SerializeField] protected float moveFrequency =1f;

    //Hur mycket den r�r sig i Y-led (ner). Det h�r �r h�lften av v�rdet d� den r�r sig 2 g�nger innan den v�nder.
    [SerializeField] protected float down = -0.25f;

    [Tooltip("How low does the enemy go before it dies ")]
    [SerializeField] protected float minYpos;
    [SerializeField] protected float startYPos = 6;

    /*
    //Original positionens x-v�rde (�r n�r den �r l�ngst till v�nster)
    [Tooltip("Left most point on the x axis")]
    [SerializeField] protected float maxLeftPosX;
    [Tooltip("Right most point on the x axis")]
    [SerializeField] protected float maxRightPosX;
    */

    protected Vector2 targetPos;

    protected float timer;
    [SerializeField] protected Vector2 gainPoints;
    [Tooltip("Shake intensity and duration")]
    [SerializeField] protected Vector2 shake;
    [SerializeField] ParticleSystem constantParticles;
    [SerializeField] ParticleSystem changeDirectionParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] GameObject deathExplosion;

    protected Rigidbody2D rb;

    public bool GivePoints;
    public bool GiveNewWeapon;
    [SerializeField] GameObject newWeaponSpawner;

    private bool _dead;
    private LayerMask _playerLayer;

    [SerializeField] private Animator hurtAnim;
    [SerializeField] protected LayerMask wallLayer;
    [SerializeField] protected float offsetSizeAmount = 1;

    public bool SpawnedByBossOrSpawner { get; set; }
    private int _timesMoved;

    //private bool _moveDown;

    //Score score;

    //Camera cam;

    //CameraEffects cameraEffects;

    //Det h�r �r f�r att de ska "blinka" n�r de r�r sig.
    //private Color color;
    //[SerializeField] SpriteRenderer rend;

    // Start is called before the first frame update

    private void Awake()
    {
        EnemyStorage.AddEnemy(this);
        _playerLayer = GameManager.Instance.PlayerLayer;

        if (Random.Range(0, 2) == 1)
        {
            moveSideSpeed *= -1;
        }
    }

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        targetPos = transform.position;
        
        if (!SpawnedByBossOrSpawner)
        {
            targetPos.y = startYPos + (Random.value - 0.5f);
        }
        else
        {
            targetPos.y -= offsetSizeAmount * 2;
        }

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

    private void OnDestroy()
    {
        EnemyStorage.RemoveEnemy(gameObject);
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

        /*
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
        */

        if (timer >= moveFrequency)
        {
            /*
            if (_moveDown)
            {
                _moveDown = false;
                
                targetPos += new Vector2(0, down); // Ramla en rad ner
                
                //Byter h�ll
                moveSideSpeed *= -1;
                changeDirectionParticles.Play();
            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(targetPos, moveSideSpeed > 0 ? Vector2.right : Vector2.left, Mathf.Abs(moveSideSpeed) + offsetSizeAmount, wallLayer);

                if (hit)
                {
                    targetPos = new Vector2(hit.point.x - (offsetSizeAmount * Mathf.Sign(moveSideSpeed)), targetPos.y);

                    _moveDown = true;
                }
                else
                {
                    targetPos += new Vector2(moveSideSpeed, 0); // G� �t sidan
                }
            }
            */

            RaycastHit2D hit = Physics2D.Raycast(targetPos, moveSideSpeed > 0 ? Vector2.right : Vector2.left, Mathf.Abs(moveSideSpeed) + (offsetSizeAmount / 2), wallLayer);

            // Fiende spawnade och g�r ned p� direkten
            if (hit && _timesMoved <= 0)
            {
                moveSideSpeed *= -1; // bara byt h�ll ist�llet f�r allt annat
            }

            if (hit && _timesMoved > 0)
            {
                targetPos += new Vector2(0, down); // Ramla en rad ner

                //Byter h�ll
                moveSideSpeed *= -1;
                changeDirectionParticles.Play();
            }
            else
            {
                targetPos += new Vector2(moveSideSpeed, 0); // G� �t sidan
            }

            _timesMoved++;

            /*
            //Ifall den �r vid extrem punkterna skall den v�nda om och hoppa ner. // Tv� "steg" fr�n original positionen, eller i origianl positionen (b�da �r ett "steg" fr�n mitten)
            if (futurePos.x <= maxLeftPosX && moveSideSpeed < 0 || futurePos.x >= maxRightPosX && moveSideSpeed > 0)
            {
                targetPos += new Vector2(0, down); // Ramla en rad ner
                                                   //Byter h�ll
                moveSideSpeed *= -1;
                changeDirectionParticles.Play();
            }
            else
            {
            }
            */

            timer = 0;
        }

        if (transform.position.y <= minYpos) // Vi tog oss genom spelarens niv�
        {
            HitPlayer(); // Skada spelaren
        }

        transform.position = Vector3.Lerp(transform.position, targetPos, moveLerp * Time.deltaTime);
    }

    //Det h�r h�nder n�r fienderna r�r n�got som inte �r spelaren. (Aktiveras i "OnCollisionEnter...").
    public virtual void Die(bool givePoints)
    {
        if (_dead)
        {
            return;
        }

        _dead = true;

        //Kopierat fr�n Score-script. Vet inte hur po�ngen funkar, s� ifall det h�r �r fel �r det bara att �ndra.
        //Just nu kommer det ett error meddelande, som s�ger att det �r fel p� Score 63, men vet inte om det �r pga den h�r koden
        //Score koden, eller ifall det �r f�r att n�got saknas???
        //Vector3 mousePos = Input.mousePosition;
        //mousePos.z = -cam.transform.position.z;

        if (GivePoints && givePoints)
        {
            Score.AddPoints(Mathf.RoundToInt(Random.Range(gainPoints.x, gainPoints.y)), transform.position); // Give points
        }
        if (GiveNewWeapon)
        {
            Instantiate(newWeaponSpawner, transform.position - new Vector3(0, 1, 0), Quaternion.identity);
        }

        // G�r camera effects
        CameraEffects.Shake(shake.x, shake.y);
        CameraEffects.Zoom(65, 0.5f, Vector3.zero);
        CameraEffects.Flash(0.5f, new Color(1, 1, 1, 0.5f));

        SoundManager.PlaySound("Enemy Die");

        // Deatach constant particles from parent so they don't die with the enemy
        constantParticles.transform.SetParent(null);
        constantParticles.Stop();
        Destroy(constantParticles.gameObject, 5);

        deathParticles.Play(); // Play death particles
        deathParticles.transform.parent = null; // Deatach from parent

        Destroy(deathParticles.gameObject, 3); //Destroy them later

        // Death explosion
        deathExplosion.gameObject.SetActive(true);
        deathExplosion.transform.SetParent(null);
        Destroy(deathExplosion, 1);

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
        else
        {
            SoundManager.PlaySound("Basement Fart");

            hurtAnim.SetTrigger("Hurt");
        }
    }

    public virtual void HitPlayer() // Enemy hit player or player snuck past player
    {
        Die(false);

        PlayerMove.Instance.HitPlayer();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (_playerLayer == (_playerLayer | (1 << collision.gameObject.layer)))
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