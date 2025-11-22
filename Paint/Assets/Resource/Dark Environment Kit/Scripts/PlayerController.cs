using UnityEngine;

namespace LimboStyleEnvironment
{
    public class PlayerController : MonoBehaviour
    {
        public float movingSpeed;
        public float jumpForce;
        private float moveInput;

        private bool isGrounded;
        public Transform groundCheck;
        private Vector2 direction;

        private Rigidbody2D rb;
        private Animator animator;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            moveInput = Input.GetAxis("Horizontal");
            direction = transform.right * moveInput;

            if (moveInput != 0)
            {
                animator.SetInteger("playerState", 1); // Turn on run animation
            }
            else
            {
                if (isGrounded) animator.SetInteger("playerState", 0); // Turn on idle animation
            }

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            }
            if (!isGrounded) animator.SetInteger("playerState", 2); // Turn on jump animation

            CheckGround();
        }

        private void FixedUpdate()
        {
            rb.velocity = new Vector2(direction.x * movingSpeed, rb.velocity.y);
        }

        private void CheckGround()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, 0.6f);
            isGrounded = colliders.Length > 1;
        }
    }
}
