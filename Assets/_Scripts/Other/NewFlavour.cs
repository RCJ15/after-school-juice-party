using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewFlavour : MonoBehaviour
{
    [SerializeField] float fallSpeed;
    SpriteRenderer spriteRend;
    Rigidbody2D rb;
    PlayerShootManager shootManager;
    int choosenFlavour;

    [SerializeField] private LayerMask playerLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRend = GetComponent<SpriteRenderer>();

        shootManager = PlayerShootManager.Instance;

        choosenFlavour = shootManager.GetRandomWeapon(); // Random weapon
    }

    private void Update()
    {
        if (transform.position.y <= -10)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.up * -fallSpeed;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (playerLayer == (playerLayer | (1 << col.gameObject.layer)))
        {
            GiveFlavour();

            Score.AddPoints(1000, transform.position);

            SoundManager.PlaySound("Powerup", 1);
            Destroy(gameObject);
        }
    }

    void GiveFlavour()
    {
        shootManager.ChangeWeapon(shootManager.SelectedWeapon, choosenFlavour);
    }
}