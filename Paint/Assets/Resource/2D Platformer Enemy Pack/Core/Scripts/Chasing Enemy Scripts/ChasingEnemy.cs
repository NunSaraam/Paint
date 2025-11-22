using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyPack
{
    public class ChasingEnemy : MonoBehaviour
    {
        // Reference to your custom sound manager
        public SoundManager soundManager;
        private bool facingRight = false;
        public float speed;

        // Reference to the player (found dynamically)
        private Transform player;

        // Detection range to start chasing
        public float lineOfSite;

        [SerializeField] private AudioClip DeathSound;
        [SerializeField] private AudioClip FlySound;

        [SerializeField] private AudioSource audioSource;

        // Bounce force applied to player when enemy is stomped
        public float bounceForce = 10f;

        // Flag to prevent multiple death triggers
        public bool isDead;

        // Particle system prefab to play on death
        public ParticleSystem deathParticlesPrefab;

        void Start()
        {
            TryFindPlayer();

            // Play flying sound every 4 seconds
            InvokeRepeating(nameof(PlayFlySound), 0f, 4f);

            // Try to re-find the player every 2 seconds in case they spawn later
            InvokeRepeating(nameof(TryFindPlayer), 1f, 2f);
        }

        // Attempts to locate the player in the scene
        void TryFindPlayer()
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }

        // Plays flying sound periodically (only if alive and player exists)
        private void PlayFlySound()
        {
            if (!isDead && player != null)
            {
                audioSource.PlayOneShot(FlySound);
            }
        }

        void Update()
        {
            // Skip logic if dead or no player in scene
            if (player == null || isDead)
                return;

            // If the player is within range, move toward them
            float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
            if (distanceFromPlayer < lineOfSite)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }

            // Flip enemy to face the player
            float directionToPlayer = player.position.x - transform.position.x;
            if (directionToPlayer < 0 && facingRight)
            {
                Flip();
            }
            else if (directionToPlayer > 0 && !facingRight)
            {
                Flip();
            }
        }

        // Shows the detection radius in the editor
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, lineOfSite);
        }

        // Handles death behavior: sound, particles, and destroy
        public void Die()
        {
            if (!isDead)
            {
                soundManager?.PlayDeathSoundFollowingEnemy();

                if (deathParticlesPrefab != null)
                {
                    ParticleSystem deathParticles = Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
                    deathParticles.Play();
                }

                audioSource?.PlayOneShot(DeathSound);
                Destroy(gameObject);
                isDead = true;
            }
        }

        // Checks collision with player to trigger bounce and death
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!isDead && collision.gameObject.CompareTag("Player"))
            {
                Rigidbody2D playerRB = collision.gameObject.GetComponent<Rigidbody2D>();
                if (playerRB != null)
                {
                    playerRB.velocity = new Vector2(playerRB.velocity.x, bounceForce);
                    Die();
                }
            }
        }

        // Flips the enemy horizontally
        void Flip()
        {
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
    }
}
