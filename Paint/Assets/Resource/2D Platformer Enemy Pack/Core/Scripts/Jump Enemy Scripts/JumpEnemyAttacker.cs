using UnityEngine;
using System.Collections;
using Cinemachine;
namespace EnemyPack
{
    public class JumpEnemyAttacker : MonoBehaviour
    {
        public float JumpLenght; // Controls how far the enemy will jump towards the player.
        public SoundManager soundManager; // Reference to the SoundManager to play sounds.
        public bool isMoving; // Keeps track of whether the enemy is moving or not.
        public ParticleSystem JumpParticule; // Particle system that plays when the enemy jumps.
        public CinemachineImpulseSource ImpulseSourceEnemyJump; // Used to trigger the camera shake when the enemy jumps.

        public float bounceForce; // Determines the force with which the player is bounced upon collision with the enemy.
        private bool isDead = false; // Tracks whether the enemy is dead.
        public ParticleSystem deathParticlesPrefab; // Particle system to play upon the enemy's death.

        [Header("For Patrolling")]
        [SerializeField] float moveSpeed; // Speed at which the enemy moves while patrolling.
        private float moveDirection = -1; // Direction the enemy is moving in, initially moving left.
        private bool facingRight = false; // Tracks whether the enemy is facing right or left.
        [SerializeField] Transform groundCheckPoint; // Position to check if the enemy is on the ground.
        [SerializeField] Transform wallCheckPoint; // Position to check if there's a wall in front of the enemy.
        [SerializeField] LayerMask groundLayer; // Defines which layer is considered as ground.
        [SerializeField] LayerMask WallLayer; // Defines which layer is considered as a wall.
        [SerializeField] float circleRadius; // Radius used for ground and wall detection.
        private bool checkingGround; // Boolean to check if the enemy is on the ground.
        private bool checkingWall; // Boolean to check if there's a wall in front of the enemy.
        private Animator enemyAnim; // Animator component to control animations.
        private bool CanFlip = true;//Checking if the Enemy can Check.

        [Header("For JumpAttacking")]
        [SerializeField] float jumpHeight; // Controls the height of the enemy's jump.
        [SerializeField] Transform player; // Reference to the player character.
        [SerializeField] Transform groundCheck; // Position to check if the enemy is on the ground.
        [SerializeField] Vector2 boxSize; // Size of the box used for ground checking.
        private bool isGrounded; // Tracks if the enemy is on the ground.
        private bool isAttacking; // Tracks if the enemy is in the middle of an attack.
        private float preAttackDelay = 0.5f; // Delay before the enemy attacks.

        [Header("For SeeingPlayer")]
        [SerializeField] Vector2 lineOfSite; // Defines the size and range of the enemy's line of sight.
        [SerializeField] LayerMask playerLayer; // Defines which layer is considered as the player.
        private bool canSeePlayer; // Tracks if the enemy can see the player.
        private Rigidbody2D enemyRB; // Reference to the Rigidbody2D of the enemy for physics manipulation.
        [SerializeField] private AudioClip JumpSound; // Sound played during the jump attack.
        [SerializeField] private AudioClip WalkSound; // Sound played while patrolling.
        [SerializeField] private AudioSource audioSource; // AudioSource for playing sound effects.
        [SerializeField] private AudioSource audioSource2; // Additional AudioSource for sound effects.

        // Start is called before the first frame update
        void Start()
        {
            enemyRB = GetComponent<Rigidbody2D>(); // Gets the Rigidbody2D component attached to the enemy.
            enemyAnim = GetComponent<Animator>(); // Gets the Animator component attached to the enemy.
            ImpulseSourceEnemyJump = GetComponent<CinemachineImpulseSource>(); // Gets the CinemachineImpulseSource component for camera shake.
        }

        // FixedUpdate is called at a fixed interval, suitable for physics calculations
        void Update()
        {
            if (player == null)
            {
                enabled = false;
                return; //Quit Update
            }
                // Check if the enemy is on the ground or touching a wall.
            checkingGround = Physics2D.OverlapCircle(groundCheckPoint.position, circleRadius, groundLayer);
            checkingWall = Physics2D.OverlapCircle(wallCheckPoint.position, circleRadius, WallLayer);
            isGrounded = Physics2D.OverlapBox(groundCheck.position, boxSize, 0, groundLayer);
            canSeePlayer = Physics2D.OverlapBox(transform.position, lineOfSite, 0, playerLayer);
            float playerPosition = player.position.x - transform.position.x;

            // If the enemy is on the ground, it can patrol or attack.
            if (isGrounded)
            {
                if (!isAttacking && !canSeePlayer)
                {
                    Patrolling(); // If the enemy can't see the player, it patrols.
                    enemyAnim.Play("JumpEnemyWalk"); // Play walking animation.
                }

                if (canSeePlayer && !isAttacking)
                {
                    StartCoroutine(PreAttackDelay()); // If the player is seen, initiate attack after a delay.
                }
                if (!audioSource2.isPlaying)
                {
                    
                    if (!isAttacking)
                    {
                        
                        audioSource2.PlayOneShot(WalkSound);

                    }
                }
                else
                {
                    isMoving = false;
                    if (isAttacking || !isGrounded)
                  
                {
                    audioSource2.Stop();
                }
                }
            }
            else
            {
                enemyAnim.Play("EnemyJumpAir"); // Play the jumping animation.    
            }

        }

        // Patrol behavior: move left or right, check for ground and walls
        void Patrolling()
        {
            isMoving = true;
            if (!checkingGround || checkingWall) // If no ground is detected or a wall is detected, flip the enemy.
            {
                Flip();
            }
            else // Normal patrolling movement.
            {
                enemyRB.velocity = new Vector2(moveSpeed * moveDirection, enemyRB.velocity.y);
            }
        }

        // Execute the jump attack
        private void JumpAttack()
        {
            float distanceFromPlayer = player.position.x - transform.position.x;

            if (isGrounded)
            {
                audioSource.PlayOneShot(JumpSound); // Play the jump sound.
                CreateJumpPS(); // Trigger the jump particles.

                // Trigger camera shake when the enemy jumps.
                ImpulseSourceEnemyJump.GenerateImpulse();

                // Adjust jump distance based on the player's position.
                float jumpDistance = distanceFromPlayer * JumpLenght;

                // Apply force to the enemy's Rigidbody to make it jump.
                enemyRB.AddForce(new Vector2(jumpDistance, jumpHeight), ForceMode2D.Impulse);
            }
        }

        // Flip the enemy's direction, but wait 0.3 seconds before allowing another flip
        void Flip()
        {
            if (CanFlip)
            {
                CanFlip = false;
                if (isDead) return; // Prevent flipping if the enemy is dead.

                // Delay to prevent rapid flipping.


                moveDirection *= -1; // Reverse the movement direction.
                facingRight = !facingRight; // Toggle the facing direction.
                transform.Rotate(0, 180, 0); // Rotate the enemy by 180 degrees to face the opposite direction.
                StartCoroutine(FlipDelay());
            }

        }

        // Coroutine to add a delay after flipping.
        private IEnumerator FlipDelay()
        {

            yield return new WaitForSeconds(1f); // Wait for 0.3 seconds before the enemy can flip again.
            CanFlip = true;
        }

        // Delay before the enemy attacks after seeing the player.
        private IEnumerator PreAttackDelay()
        {
            isAttacking = true;

            // Check if the player is behind and flip the enemy accordingly.
            if ((player.position.x < transform.position.x && facingRight) ||
                (player.position.x > transform.position.x && !facingRight))
            {
                Flip();
            }

            enemyAnim.Play("EnemyJump"); // Play jump animation before attacking.
            yield return new WaitForSeconds(preAttackDelay); // Wait before executing the attack.

            JumpAttack(); // Perform the jump attack.

            // Wait until the enemy is grounded again.
            while (!isGrounded)
            {
                yield return null;
            }

            // Wait a bit before allowing another attack.
            yield return new WaitForSeconds(0.5f);
            isAttacking = false;
        }

        // Draw gizmos in the editor for debugging purposes
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(groundCheckPoint.position, circleRadius); // Draw the ground check radius.
            Gizmos.DrawWireSphere(wallCheckPoint.position, circleRadius); // Draw the wall check radius.

            Gizmos.color = Color.green;
            Gizmos.DrawCube(groundCheck.position, boxSize); // Draw the ground check box.

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, lineOfSite); // Draw the line of sight detection area.
        }

        // Enemy dies and triggers death particles and sound.
        public void Die()
        {
            if (!isDead)
            {
                ParticleSystem deathParticles = Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
                soundManager.PlayDeathSoundJumpEnemy(); // Play the death sound.
                deathParticles.Play(); // Play the death particles.
                Destroy(gameObject); // Destroy the enemy object.
                isDead = true;
            }
        }

        // Handle collision with the player
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!isDead && collision.gameObject.CompareTag("Player"))
            {
                Rigidbody2D playerRB = collision.gameObject.GetComponent<Rigidbody2D>();
                if (playerRB != null)
                {
                    playerRB.velocity = new Vector2(playerRB.velocity.x, bounceForce); // Bounce the player.
                    Die(); // Enemy dies upon collision with the player.
                }
            }
        }

        // Create the jump particle system
        void CreateJumpPS()
        {
            JumpParticule.Play(); // Play the jump particles.
        }
    }
}