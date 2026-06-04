using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PowerUpPickup : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent onPickedUpEvent = new UnityEvent();

    [Header("Settings")]
    [SerializeField] private bool destroyOnPickup = true;   // Should the power-up disappear when collected?

    [Header("References")]
    [SerializeField] private ParticleSystem pickupParticle;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Touched by {other.gameObject.name} & tag of player = {other.tag}");
        // Check if the object that touched this power-up is the player
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Power-Up collected by {other.gameObject.name}");

            // Trigger the event (for the spawner to reduce count)
            onPickedUpEvent?.Invoke();

            // Destroy the power-up
            if (destroyOnPickup)
            {
                StartCoroutine(WaitForParticleCompletionAndDestroy());
            }
        }
    }

    private IEnumerator WaitForParticleCompletionAndDestroy()
    {
        yield return new WaitForSeconds(pickupParticle.duration);
        Destroy(gameObject);
    }

    // Optional: Also support mouse click or other interactions
    private void OnMouseDown()
    {
        if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < 5f)
        {
            Debug.Log("Power-Up collected via click");
            onPickedUpEvent?.Invoke();
            Destroy(gameObject);
        }
    }
}