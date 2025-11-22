using UnityEngine;
using System.Collections;
using Cinemachine;

namespace EnemyPack
{
    // This class controls a magical enemy's behavior, including attacking the player and dying
    public class MagicEnemy : MonoBehaviour
    {
        public SoundManager soundManager; // Reference to the sound manager for sound effects

        private bool facingRight = false; // Whether the enemy is facing right
        public Transform playerTransform; // Reference to the player's transform
        private Animator enemyAnim; // Reference to the Animator component

        [SerializeField] private AudioClip MagicAttackSound; // Sound clip for when the enemy attacks
        [SerializeField] private AudioClip MagicSound; // Background sound clip for the enemy
        [SerializeField] private AudioSource audioSource; // AudioSource used to play the clips
        public CinemachineImpulseSource ImpulseSourceEnemyTank; // Impulse source for screen shake

        public float bounceForce; // Bounce force applied to player on contact
        private bool isDead = false; // Whether the enemy is dead

        public ParticleSystem deathParticlesPrefab; // Particle system to play on death

        [Header("For Seeing Player")]
        [SerializeField] Vector2 lineOfSite; // Area in which the enemy can see the player
        [SerializeField] LayerMask playerLayer; // Layer that identifies the player
        private bool canSeePlayer; // Whether the enemy currently sees the player
        public float timeSinceLastAttack = 1.2f; // Timer since last attack
        public float attackInterval = 1.2f; // Minimum interval between attacks

        public GameObject attackObjectPrefab; // The magic projectile prefab

        private bool isAttacking; // Whether the enemy is currently attacking

        [Header("Attack Offset")]
        public float xOffset = 8f;  // Horizontal offset for attack spawn point
        public float yOffset = -4f; // Vertical offset for attack spawn point

        private void Start()
        {
            // Get references to required components
            ImpulseSourceEnemyTank = GetComponent<CinemachineImpulseSource>();
            enemyAnim = GetComponent<Animator>();
            InvokeRepeating("PlayTankSound", 0f, 4f); // Loop the tank sound every 4 seconds

            // Find the player's transform by tag
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Plays the tank's ambient sound if not dead
        private void PlayTankSound()
        {
            if (!isDead)
            {
                audioSource.PlayOneShot(MagicSound);
            }
        }

        // Coroutine that handles the entire attack sequence
        private IEnumerator WaitAttack()
        {
            isAttacking = true;

            // Play attack animation
            enemyAnim.Play("MagicAttack");

            // Small delay to match animation timing
            yield return new WaitForSeconds(0.6f);

            // Play attack sound
            audioSource.PlayOneShot(MagicAttackSound);

            // Short wait before projectile instantiation
            yield return new WaitForSeconds(0.1f);

            // Calculate attack position based on facing direction and offsets
            float adjustedXOffset = facingRight ? xOffset : -xOffset;
            Vector2 attackPosition = transform.position + new Vector3(adjustedXOffset, yOffset, 0f);

            // Instantiate and launch projectile
            GameObject attackObject = Instantiate(attackObjectPrefab, attackPosition, Quaternion.identity);
            Vector2 attackDirection = facingRight ? Vector2.right : Vector2.left;
            attackObject.GetComponent<Projectile>().SetAttackDirection(attackDirection);
            Destroy(attackObject, 1.2f); // Destroy projectile after duration

            timeSinceLastAttack = 0f;

            // Screen shake effect
            ImpulseSourceEnemyTank.GenerateImpulse();
            isAttacking = false;
        }

        private void Update()
        {
            // Play idle animation when not attacking
            if (!isAttacking)
            {
                enemyAnim.Play("MagicIdle");
            }

            // Check if the player is within line of sight
            canSeePlayer = Physics2D.OverlapBox(transform.position, lineOfSite, 0, playerLayer);

            // Attack if the player is visible and enough time has passed
            if (canSeePlayer && !isAttacking)
            {
                timeSinceLastAttack += Time.deltaTime;

                if (timeSinceLastAttack >= attackInterval)
                {
                    Attack();
                }
            }

            // Flip direction to face the player
            if (canSeePlayer && !isAttacking)
            {
                float playerPosition = playerTransform.position.x - transform.position.x;
                if (playerPosition < 0 && facingRight)
                {
                    Flip();
                }
                else if (playerPosition > 0 && !facingRight)
                {
                    Flip();
                }
            }
        }

        // Start the attack coroutine
        private void Attack()
        {
            StartCoroutine(WaitAttack());
        }

        // Visual aid for line of sight in the editor
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, lineOfSite);
        }

        // Handles enemy death logic
        public void Die()
        {
            if (!isDead)
            {
                ImpulseSourceEnemyTank.GenerateImpulse(); // Screen shake on death

                // Instantiate and play death particles
                ParticleSystem deathParticles = Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
                deathParticles.Play();

                Destroy(gameObject); // Destroy the enemy object
                isDead = true;
            }
        }

        // Collision detection with player
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!isDead && collision.gameObject.CompareTag("Player"))
            {
                soundManager.PlayDeathSoundMagic(); // Play death sound

                // Apply bounce to player
                Rigidbody2D playerRB = collision.gameObject.GetComponent<Rigidbody2D>();
                if (playerRB != null)
                {
                    playerRB.velocity = new Vector2(playerRB.velocity.x, bounceForce);
                    Die();
                }
            }
        }

        // Flips the enemy's facing direction
        void Flip()
        {
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
    }
}
