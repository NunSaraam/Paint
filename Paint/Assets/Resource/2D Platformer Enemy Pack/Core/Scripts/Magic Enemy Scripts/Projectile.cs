using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace EnemyPack
{

    // This class handles the behavior of a magic projectile
    public class Projectile : MonoBehaviour
    {
        public SoundManager soundManager; // Reference to the sound manager for playing sound effects
        public float attackSpeed = 10f; // Speed at which the projectile moves
        private Vector2 attackDirection; // Direction in which the projectile will travel
        public ParticleSystem ProjectileParticle; // Particle system played when the projectile is launched
        public ParticleSystem ImpactParticlesPrefab; // Particle system prefab for the impact effect
        public CinemachineImpulseSource ImpulseSourceMagic;

        private void Start()
        {
            // Get the CinemachineImpulseSource component attached to the projectile
            ImpulseSourceMagic = GetComponent<CinemachineImpulseSource>();

            // Get the Rigidbody2D component to control movement via physics
            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // Set the velocity of the projectile in the direction of the attack
                rb.velocity = attackDirection.normalized * attackSpeed;

                // Play the launch particle effect
                ProjectileParticle.Play();

                // Calculate the angle of rotation based on the direction of travel
                float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;

                // Rotate the projectile so it faces the direction it's moving
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
            else
            {
                Debug.LogWarning("Rigidbody2D is not attached to the projectile.");
            }
        }

        private void Update()
        {
            // You can add additional projectile behaviors here if needed
        }

        // Method to set the direction of the projectile externally
        public void SetAttackDirection(Vector2 direction)
        {
            attackDirection = direction;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Check if the projectile collided with a wall, player, or the ground
            if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Ground"))
            {
                // Play explosion sound effect if soundManager is assigned
                if (soundManager != null)
                 {
                       soundManager.ProjectileExplosion();
                 }

                // Instantiate and play impact particles at the point of collision
                if (ImpactParticlesPrefab != null)
                {
                    Instantiate(ImpactParticlesPrefab, transform.position, Quaternion.identity).Play();
                }

                // Trigger camera impulse (screen shake) if available
                if (ImpulseSourceMagic != null)
                {
                    ImpulseSourceMagic.GenerateImpulse();
                }

                // Destroy the projectile after the impact
                Destroy(gameObject);
            }
        }
    }
}
