using UnityEngine;

public class Explode : MonoBehaviour
{
    [SerializeField] GameObject explotion;
    [SerializeField] Vector3 explosionSize;
    [SerializeField] protected ParticleSystem DeathParticles;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") )//|| collision.CompareTag("Wall")) // Hit enemy or wall
        {
            GameObject newExplosion = Instantiate(explotion, transform.position, Quaternion.identity, null); // Spawn new explosion
            newExplosion.transform.localScale = explosionSize; // Set its Size

            // Some code made by Ruben to deatatch the particlesystem
            DeathParticles.Play();
            DeathParticles.transform.SetParent(null);

            if (DeathParticles.TryGetComponent<KillObjectAfterTime>(out _))
            {
                return;
            }

            ParticleSystem.MainModule main = DeathParticles.main;
            AnimationCurve curve = main.startLifetime.curveMax;
            Destroy(DeathParticles, main.duration + main.startLifetime.constantMax + (curve != null ? curve.Evaluate(1) : 0)); // Destroy after some time
        }
    }
}