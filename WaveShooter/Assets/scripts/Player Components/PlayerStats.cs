using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] public HealthBar healthBar;

    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;

        healthBar.SetSliderMax(maxHealth);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        healthBar.SetSlider(currentHealth);
        if (currentHealth <= 0)
            EndGame();
    }

    private void EndGame()
    {
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(1f);
        }    
    }
}
