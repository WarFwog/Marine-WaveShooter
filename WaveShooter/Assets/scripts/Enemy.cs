using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private float _maxHealth = 50f;     // Maximum health of this enemy
    [SerializeField] private float _currentHealth;       // Current health (will be set in Start)

    [Header("Effects (Optional)")]
    [SerializeField] private ParticleSystem _deathParticles;   // Particle effect when enemy dies

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    /// <summary>
    /// Called when this enemy takes damage (usually from a bullet).
    /// </summary>
    public void TakeDamage(float amount)
    {
        _currentHealth -= amount;

        if (_currentHealth <= 0f)
        {
            Die();
        }
    }

    /// <summary>
    /// Handles enemy death: plays effects and destroys the GameObject (de-spawn).
    /// </summary>
    private void Die()
    {
        // Optional: Play death particles
        if (_deathParticles != null)
        {
            Instantiate(_deathParticles, transform.position, Quaternion.identity);
        }

        // De-spawn / Destroy the enemy
        Destroy(gameObject);
        
        Debug.Log(gameObject.name + " has been destroyed!");
    }

    /// <summary>
    /// Simple way to check if this object is an enemy from other scripts.
    /// </summary>
    public bool IsAlive()
    {
        return _currentHealth > 0f;
    }
}