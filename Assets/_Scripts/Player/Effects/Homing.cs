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
    [SerializeField] Crosshair crossHair;
    [SerializeField] string foundTargetSoundEffect;

    [Space]
    [SerializeField] private bool focusPlayer;
    bool _foundTarget;
    Transform _target;

    private Rigidbody2D _rb;
    private PlayerMove _player;

    private Boss _boss;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = PlayerMove.Instance;
        _boss = Boss.Instance;

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

        InvokeRepeating(focusPlayer ? nameof(SetPlayerTarget) : nameof(SetTarget), 0, 0.05f);
    }


    private void FixedUpdate()
    {
        if (!_foundTarget)
        {
            return;
        }

        if (_target == null)
        {
            _foundTarget = false;
            return;
        }

        // Home towards the target
        Vector2 direction = (Vector2)_target.position - _rb.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        _rb.angularVelocity = -rotateAmount * rotateSpeed;
    }

    private void SetTarget()
    {
        if (_foundTarget)
        {
            return;
        }

        float oldDistance = minDistance; // Start of very big

        _target = null;

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) // Find closest enemy
        {
            // Is boss
            if (enemy.layer == 13)
            {
                // Ignore dead boss
                if (_boss.Dead)
                {
                    continue;
                }
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
            _foundTarget = false;
            return;
        }

        OnFindTarget();
    }

    private void OnFindTarget()
    {
        if (!string.IsNullOrEmpty(foundTargetSoundEffect))
        {
            SoundManager.PlaySound(foundTargetSoundEffect);
        }

        _foundTarget = true;

        if (crossHair == null)
        {
            return;
        }

        crossHair.Target = _target;
        crossHair.gameObject.SetActive(true);
        crossHair.transform.SetParent(null);
        crossHair.transform.position = _target.position;

        // StartCoroutine( Fade(newCrossHair.GetComponent<SpriteRenderer>())); // Fade out
        Destroy(crossHair, 3); // Destroy after 3 seconds
    }

    // Used with bullets with "focusPlayer" set to true
    private void SetPlayerTarget()
    {
        if (_foundTarget)
        {
            return;
        }

        _target = null;

        if (Vector2.Distance(_player.transform.position, transform.position) > minDistance)
        {
            return;
        }

        _target = _player.transform;

        OnFindTarget();
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