using UnityEngine;

namespace SceneScript
{
    public class PlayerMovementDemosScenes : MonoBehaviour
    {
        public float moveSpeed = 5f; // Player movement speed
        private Rigidbody2D rb; // Reference to the Rigidbody2D component for physics-based movement
        public GameObject Window; // Retry window UI

        private void Start()
        {
            // Get the Rigidbody2D attached to this GameObject
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            // Get user input for horizontal and vertical movement
            float horizontal = Input.GetAxis("Horizontal"); // Left/Right arrows or A/D
            float vertical = Input.GetAxis("Vertical");     // Up/Down arrows or W/S

            // Calculate the movement vector
            Vector2 movement = new Vector2(horizontal, vertical) * moveSpeed;

            // Apply movement through the Rigidbody2D velocity
            rb.velocity = movement;
        }

        // Trigger detection
        private void OnTriggerEnter2D(Collider2D other)
        {
            // Check if the player collided with an object tagged "DeathZone"
            if (other.CompareTag("DeathZone"))
            {
                // Destroy the player object
                Destroy(gameObject);

                // Show the retry window
                RetryWindow();
            }
        }

        // Function to display the retry window
        public void RetryWindow()
        {
            Window.SetActive(true);
        }
    }
}
