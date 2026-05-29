using UnityEngine;
using UnityEngine.Events;

public class PowerUpPickup : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent OnPickedUpEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Power-Up Collected by Player!");
            
            OnPickedUpEvent?.Invoke();
            Destroy(gameObject);
        }
    }
}