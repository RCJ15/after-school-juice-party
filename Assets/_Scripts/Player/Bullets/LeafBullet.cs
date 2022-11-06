using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafBullet : Bullet
{
    [SerializeField] float speedDown;
    [SerializeField] float damageDown;
    [SerializeField] float maxHeight;
    [SerializeField] float shakeIntensity;
    [SerializeField] float shakeDuration;

    bool _switched;

    protected override void Update()
    {
        base.Update();
        if (transform.position.y >= maxHeight && !_switched)
        {
            StartCoroutine(Speed());
            damage = damageDown;
            StartCoroutine(Shake());
            _switched = true;
        }
    }
    IEnumerator Speed()
    {
        float timer = 0.5f;
        float oldSpeed = speed;
        while (timer>0)
        {
            timer = -Time.deltaTime;
            speed = Mathf.Lerp(oldSpeed, -speedDown, 1 - (timer / 0.5f));
            yield return null;
        }
        speed = -speedDown;
    }
    IEnumerator Shake()
    {
        float timer = shakeDuration;
        float StartX = transform.position.x;

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            // float t = Easing.InOutBack(1 - (timer / shakeDuration));
            float t = 1 - (timer / shakeDuration);
            float x = StartX + Mathf.Lerp(-shakeIntensity, shakeIntensity, t);
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
            yield return null;
        }
        timer = shakeDuration;
        while (timer > 0)
        {
            timer -= Time.deltaTime;

            //  float t = Easing.InOutBack(1 - (timer / shakeDuration));
            float t = 1 - (timer / shakeDuration);
            float x = StartX + Mathf.Lerp(shakeIntensity, -shakeIntensity, t);
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
            yield return null;
        }
        StartCoroutine(Shake());
    }
}
