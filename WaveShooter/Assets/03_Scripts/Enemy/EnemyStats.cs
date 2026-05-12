using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Boat Movement")]
    public float weight = 1f;
    public float moveSpeed = 8f;
    public float acceleration = 4f;
    public float turnSpeed = 60f;

    [Header("Combat")]
    public float shootingRange = 25f;
    public float fireRate = 1f;
    public float damage = 10f;

    private void Awake()
    {
        currentHealth = maxHealth;
    }
    
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
