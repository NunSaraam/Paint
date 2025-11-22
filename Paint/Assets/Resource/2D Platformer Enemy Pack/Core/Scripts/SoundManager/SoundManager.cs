using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyPack
{
    // This class manages playing sound effects for magic-related events like explosion and death
    public class SoundManager : MonoBehaviour
    {
        [Header("Magic Enemy")]
        [SerializeField] private AudioClip ProjectileExplosionSound; // Sound clip for explosions
        [SerializeField] private AudioClip deathSound;     // Sound clip for enemy death

        [Header("Chasing Enemy")]
        [SerializeField] private AudioClip DeathSoundFollowingEnemy;

        [Header("Jumping Enemy")]
        [SerializeField] private AudioClip DeathSoundJumpEnemy;

        [SerializeField] private AudioSource audioSource;  // Audio source used to play the clips


        // Play the magic enemy's death sound
        public void PlayDeathSoundMagic()
        {
            audioSource.PlayOneShot(deathSound);
        }

        // Play the explosion sound effect (e.g., when a projectile hits something)
        public void ProjectileExplosion()
        {
            audioSource.PlayOneShot(ProjectileExplosionSound);
        }

         public void PlayDeathSoundFollowingEnemy()
        {
            if (DeathSoundFollowingEnemy != null)
            {
                audioSource.PlayOneShot(DeathSoundFollowingEnemy);
            }
        }

        // Play the jumping enemy's death sound
        public void PlayDeathSoundJumpEnemy()
        {
            if (DeathSoundJumpEnemy != null)
            {
                audioSource.PlayOneShot(DeathSoundJumpEnemy);
            }
            else
            {
                Debug.LogWarning("DeathSoundJumpEnemy is not assigned!");
            }
        }
    }
}
