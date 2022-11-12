using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewFlavour : MonoBehaviour
{
    [SerializeField] float fallSpeed;
    SpriteRenderer spriteRend;
    Rigidbody2D rb;
    [SerializeField] PlayerShootManager shootManager;
    int choosenFlavour;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRend = GetComponent<SpriteRenderer>();

        choosenFlavour = shootManager.RandomWeapon(); // Random weapon
    }
    private void FixedUpdate()
    {
        rb.velocity = transform.up * -fallSpeed;
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            GiveFlavour();
        }
        Destroy(gameObject);
    }
    void GiveFlavour()
    {
        shootManager.AddWeapon(choosenFlavour);
    }
}