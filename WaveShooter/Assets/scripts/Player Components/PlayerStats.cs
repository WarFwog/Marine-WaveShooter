using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] public HealthBar healthBar;

    private float _currentHealth;

    private void Start()
    {
        _currentHealth = maxHealth;

        healthBar.SetSliderMax(maxHealth);
    }

    public void TakeDamage(float amount)
    {
        _currentHealth -= amount;
        healthBar.SetSlider(_currentHealth);
        if (_currentHealth <= 0)
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
