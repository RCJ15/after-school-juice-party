using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour  //Emma. Fiendernas kod
                                    //Viktigt!!! Ifall ni använder arv!!! För att något ska hända när fienderna rör något måste man ha "OnCollisionEnter..."
                                    //Som finns längst ner i koden, samt if-satsen med "collision.tag...". (Om man inte kan få in de i en funktion på något sätt...?)
{
    public float hp;

    //Hur mycket den rör sig i X-led (åt sidan). Det här är hälfte av värdet då den rör sig 2 gånger innan den vänder.
    [SerializeField] protected float moveLerp = 10f;
    [SerializeField] protected float moveSideSpeed = 1.5f;
    [SerializeField] protected float moveFrequency =1f;

    //Hur mycket den rör sig i Y-led (ner). Det här är hälften av värdet då den rör sig 2 gånger innan den vänder.
    [SerializeField] protected float down = -0.25f;

    [Tooltip("How low does the enemy go before it dies ")]
    [SerializeField] protected float minYpos;
    [SerializeField] protected float startYPos = 6;

    /*
    //Original positionens x-värde (är när den är längst till vänster)
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

    //Det här är för att de ska "blinka" när de rör sig.
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
        //Färg saker
        rend = rend != null ? rend : GetComponentInChildren<SpriteRenderer>();

        color = rend.color;

        color.a = 1;
        rend.color = color;
        */

        //Sätter originalPosX
        // maxLeftPosX = transform.position.x;
    }

    private void OnDestroy()
    {
        EnemyStorage.RemoveEnemy(gameObject);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (transform.position.y <= minYpos) // Vi tog oss genom spelarens nivå
        {
            HitPlayer(); // Skada spelaren
        }

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

        /*
        //Fienden blir osynlig
        if (timer >= moveFrequency / 2 - 0.1f && timer <= moveFrequency / 2 + 0.1f) 
        {
            color.a = 0;
            rend.color = color;
        }
        //Fienden rör sig
        else if (timer >= moveFrequency)
        {
            transform.position += new Vector3(moveSideSpeed, 0, 0); // Gå åt sidan

            //Fienden poppar fram igen (är i samma som att gå för att gå saken endast ska hända en gång)
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
                
                //Byter håll
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
                    targetPos += new Vector2(moveSideSpeed, 0); // Gå åt sidan
                }
            }
            */

            RaycastHit2D hit = Physics2D.Raycast(targetPos, moveSideSpeed > 0 ? Vector2.right : Vector2.left, Mathf.Abs(moveSideSpeed) + (offsetSizeAmount / 2), wallLayer);

            // Fiende spawnade och går ned på direkten
            if (hit && _timesMoved <= 0)
            {
                moveSideSpeed *= -1; // bara byt håll istället för allt annat
            }

            if (hit && _timesMoved > 0)
            {
                targetPos += new Vector2(0, down); // Ramla en rad ner

                //Byter håll
                moveSideSpeed *= -1;
                changeDirectionParticles.Play();
            }
            else
            {
                targetPos += new Vector2(moveSideSpeed, 0); // Gå åt sidan
            }

            _timesMoved++;

            /*
            //Ifall den är vid extrem punkterna skall den vända om och hoppa ner. // Två "steg" från original positionen, eller i origianl positionen (båda är ett "steg" från mitten)
            if (futurePos.x <= maxLeftPosX && moveSideSpeed < 0 || futurePos.x >= maxRightPosX && moveSideSpeed > 0)
            {
                targetPos += new Vector2(0, down); // Ramla en rad ner
                                                   //Byter håll
                moveSideSpeed *= -1;
                changeDirectionParticles.Play();
            }
            else
            {
            }
            */

            timer = 0;
        }

        if (transform.position.y <= minYpos) // Vi tog oss genom spelarens nivå
        {
            HitPlayer(); // Skada spelaren
        }

        transform.position = Vector3.Lerp(transform.position, targetPos, moveLerp * Time.deltaTime);
    }

    //Det här händer när fienderna rör något som inte är spelaren. (Aktiveras i "OnCollisionEnter...").
    public virtual void Die(bool givePoints)
    {
        if (_dead)
        {
            return;
        }

        _dead = true;

        //Kopierat från Score-script. Vet inte hur poängen funkar, så ifall det här är fel är det bara att ändra.
        //Just nu kommer det ett error meddelande, som säger att det är fel på Score 63, men vet inte om det är pga den här koden
        //Score koden, eller ifall det är för att något saknas???
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

        // Gör camera effects
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

        Destroy(gameObject); // Föstör objectet efter att vi har get poäng
    }

    public virtual void Hurt(float damage)
    {
        hp -= damage;
        //DPSCounter.Add(damage); // Visa vår damage per second
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
            //Spelaren förlorar liv/spelet?
            HitPlayer();
        }
        //Bullet koderna verkar ha något för det här redan?
        //Så länge den inte rör kanten borde det vara okej, men ifall vi lägger en tag på skotten borde vi ändra den här
        //för säkerhets skull
        /*else if (collision.transform.CompareTag("Bullet")) // Coliderar med ett skot
        {
            collision.TryGetComponent<Bullet>(out Bullet bullet);
            Hurt(bullet.Damage); // Skada oss
        }*/
    }
}