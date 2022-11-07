using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Homing : MonoBehaviour
{
    /*
    [Tooltip("The name of the gameobject in whitch all enemys are located.")]
    [SerializeField] string nameOfEnemysParent;
    GameObject enemysParent = null;
    */
    [Tooltip("Speed of rotation")]
    [SerializeField] float rotateSpeed = 200f;
    [SerializeField] float minDistance = 4;
    [SerializeField] GameObject crossHair;
    bool foundTarget;
    Transform _target;

    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        /*
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
        */

        InvokeRepeating(nameof(SetTarget), 0, 0.05f);
    }
    private void FixedUpdate()
    {
        if (!foundTarget)
        {
            return;
        }

        if (_target == null)
        {
            foundTarget = false;
            return;
        }

        // Home towards the target
        Vector2 direction = (Vector2)_target.position - rb.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        rb.angularVelocity = -rotateAmount * rotateSpeed;
    }

    private void SetTarget()
    {
        if (foundTarget)
        {
            return;
        }

        float oldDistance = minDistance; // Start of very big

        _target = null;

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) // Find closest enemy
        {
            if (!enemy.GetComponent<DummyEnemy>().IsAlive) // NOTE: This will have to be changed as enemies will be destroyed on death meaning that we don't have to check this - Ruben
            {
                continue;
            }

            float distance = Vector3.Distance(enemy.transform.position, transform.position); // Find the distance between us and enemy

            if (distance < oldDistance) // Find the smallest distance value
            {
                oldDistance = distance;
                _target = enemy.transform;
            }
        }

        if (_target == null) // If no enemy return
        {
            foundTarget = false;
            return;
        }

        foundTarget = true;

        if (crossHair == null)
        {
            return;
        }

        crossHair.SetActive(true);
        crossHair.transform.SetParent(null);
        crossHair.transform.position = _target.position;

        // StartCoroutine( Fade(newCrossHair.GetComponent<SpriteRenderer>())); // Fade out
        Destroy(crossHair, 3); // Destroy after 3 seconds
    }
    /*
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
    */
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