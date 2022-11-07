using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strawberry : Bullet
{
    [SerializeField] float startDamage;
    [SerializeField] float ripeDamage;
    [SerializeField] float transitionTime;

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Color startColor;
    [SerializeField] Color ripeColor;

    protected override void Awake()
    {
        base.Awake();

        StartCoroutine(Ripen());
    }
    IEnumerator Ripen()
    {
        float timer = transitionTime;
        while (timer > 0)
        {
            timer -=Time.deltaTime;

            float t = Easing.InQuad(1 - (timer / transitionTime));
            base.damage = Mathf.Lerp(startDamage, ripeDamage, t);
       spriteRenderer.color=     Color.Lerp(startColor, ripeColor, t);

            yield return null;
        }
    }
     
}
