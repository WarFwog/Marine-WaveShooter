using UnityEngine;

public class BossEnemy : EnemyStats
{
    private bool _phaseTwo;

    public override void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (!_phaseTwo && currentHealth <= maxHealth * 0.5f)
        {
            EnterPhaseTwo();
        }

        if (currentHealth <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void EnterPhaseTwo()
    {
        _phaseTwo = true;
        
        fireRate *= 2f;
        moveSpeed *= 1.5f;
        shootingRange *= 1.25f;
        
        Debug.Log("Boss entered phase 2!");
    }
}