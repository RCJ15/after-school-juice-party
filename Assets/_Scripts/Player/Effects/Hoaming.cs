using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Hoaming : MonoBehaviour
{
    [Tooltip("The name of the gameobject in whitch all enemys are located.")]
    [SerializeField] string nameOfEnemysParent;
    GameObject enemysParent = null;
    [Tooltip("Speed of roatation")]
    [SerializeField] float speed = 5f;
    [SerializeField] float rotateSpeed = 200f;
    [SerializeField] GameObject crossHair;
    bool targetAcuired;
    Transform _target;

    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject[] sceneObjects = FindObjectsOfType<GameObject>(); // All objects in scene
        List<GameObject> enemys = new List<GameObject>(); // All enemys in sceen
        enemysParent = null;

        foreach (var item in sceneObjects)
        {
            if (item.name.Contains(nameOfEnemysParent)) // Find the parent of all enemys
            {
                enemysParent = item;
            }
            if (item.tag == "Enemy") // Find all enemys
            {
                enemys.Add(item); // Add them
            }
        }
        Debug.Log(enemysParent);
        if (enemysParent == null) // No such parent
        {
            Debug.Log("Enemy parent was not found.");
            enemysParent = Instantiate(new GameObject(), new Vector3(0, 0, 0), Quaternion.identity); // Make a parent for all enemys
            enemysParent.name = nameOfEnemysParent;
            foreach (var item in enemys)
            {
                item.transform.parent = enemysParent.transform; // Add to parent
            }
            Debug.Log("Added " + enemys.Count + " Enemys to " + nameOfEnemysParent);
        }
        SetTarget();
    }
    private void FixedUpdate()
    {
        if (targetAcuired) // Hoam towards the target
        {
            Vector2 direction = (Vector2)_target.position - rb.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            rb.angularVelocity = -rotateAmount * rotateSpeed;
            rb.velocity = transform.up * speed;
        }
    }
    void SetTarget()
    {
        float oldDistance = 100000000000000; // Start of very big
        _target = null;

        foreach (Transform enemy in enemysParent.transform) // Find closest enemy
        {
            if (enemy.gameObject.activeSelf)
            {
                float distance = Vector3.Distance(enemy.position, transform.position); // Find the distance between us and enemy
                if (distance < oldDistance) // Find the smalest distance value
                {
                    oldDistance = distance;
                    _target = enemy;
                }
            }
        }
        if (_target == null) // If no enemy return
        {
            targetAcuired = false;
            return;
        }
        targetAcuired = true;

        GameObject newCrossHair = Instantiate(crossHair, _target.position, _target.rotation, _target); // Show croshair
        newCrossHair.SetActive(true);

        //   StartCoroutine( Fade(newCrossHair.GetComponent<SpriteRenderer>())); // Fade out
        Destroy(newCrossHair, 1.5f); // Destroy after 1.5 seconds
    }
    IEnumerator Fade(SpriteRenderer sprite)
    {
        float timer = 1;
        yield return new WaitForSeconds(0.25f);
        while (timer >= 0f)
        {
            float alpha = Mathf.Lerp(1, 0, Easing.Same(1 - (timer / 1)));
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
            Debug.Log("Timer" + timer + "Alpha" + alpha);
            timer -= Time.deltaTime;
            yield return null;
        }
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0);
    }
}
/* IEnumerator Rotate(Transform target, float speedOfRot)
    {
        Debug.Log("Target is: "+target);
        if (target == null) {
            Debug.Log("No target");
            yield break;
        }

        float currentRot = transform.localRotation.z;
        float targetRot = Vector3.Angle(transform.position, target.position); // Set targetrotation
        Debug.Log("Current rotation = " + currentRot + " Target rotation is = " + targetRot);
        float timer = 0;
        while (timer < speedOfRot)
        {
            timer += Time.deltaTime;
            float t = Easing(timer / speedOfRot);
            transform.localRotation=           Quaternion.Euler(   new Vector3(transform.localRotation.x, transform.localRotation.y, Mathf.Lerp(currentRot, targetRot, t)));

            Debug.Log("t=" + t+" Local rot="+transform.localRotation);
        }
        yield return null;
        static float Easing(float x)
        {
            return x;// * x * x;
        }
    }*/