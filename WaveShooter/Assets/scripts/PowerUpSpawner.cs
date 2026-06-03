using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [Header("Power-Up Settings")]
    [SerializeField] private GameObject powerUpPrefab;        // The prefab that will be spawned as a power-up
    [SerializeField] private int maxPowerUpsAtOnce = 3;       // Maximum number of power-ups that can exist on the map at the same time

    [Header("Spawn Timing")]
    [SerializeField] private float minSpawnTime = 8f;         // Minimum time (in seconds) between spawning power-ups
    [SerializeField] private float maxSpawnTime = 15f;        // Maximum time (in seconds) between spawning power-ups

    [Header("Spawn Area")]
    [SerializeField] private float spawnHeight = 1f;          // How high above the ground the power-up will spawn
    [SerializeField] private Vector2 spawnAreaSize = new Vector2(50f, 50f); // The size of the area where power-ups can spawn (X = width, Z = depth)

    // Keeps track of how many power-ups are currently active on the map
    private int currentPowerUps = 0;

    // The exact time when the next power-up should spawn
    private float nextSpawnTime;

    private void Start()
    {
        // Initialize the timer for the first power-up spawn
        SetNextSpawnTime();
        Debug.Log("PowerUpSpawner has started successfully.");
    }

    private void Update()
    {
        // Every frame we check if it's time to spawn a new power-up
        if (Time.time >= nextSpawnTime)
        {
            TrySpawnPowerUp();
        }
    }

    /// <summary>
    /// Checks if we can spawn a power-up and handles the logic before spawning.
    /// </summary>
    private void TrySpawnPowerUp()
    {
        // Guard clause: Stop if no prefab is assigned
        if (powerUpPrefab == null)
        {
            Debug.LogError("PowerUpSpawner: No Power-Up Prefab assigned!");
            return;
        }

        // Guard clause: Don't spawn if we reached the maximum limit
        if (currentPowerUps >= maxPowerUpsAtOnce)
        {
            SetNextSpawnTime();
            return;
        }

        // If all checks pass, spawn the power-up
        SpawnPowerUp();
        SetNextSpawnTime();
    }

    /// <summary>
    /// Spawns one power-up at a random position and sets up its pickup event.
    /// </summary>
    private void SpawnPowerUp()
    {
        // Get a random position on the plane
        Vector3 randomPosition = GetRandomSpawnPosition();

        // Create the power-up in the scene
        GameObject newPowerUp = Instantiate(powerUpPrefab, randomPosition, Quaternion.identity);
        
        // Parent it under the spawner for better hierarchy organization
        newPowerUp.transform.parent = transform;

        // Increase the active power-up count
        currentPowerUps++;

        Debug.Log($"Power-Up spawned at: {randomPosition}");

        // Try to connect to the pickup event so we know when it's collected
        PowerUpPickup pickupScript = newPowerUp.GetComponent<PowerUpPickup>();
        if (pickupScript != null)
        {
            pickupScript.onPickedUpEvent.AddListener(OnPowerUpPickedUp);
        }
    }

    /// <summary>
    /// Generates a random position within the spawn area on the plane.
    /// </summary>
    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 planeCenter = transform.position;

        // Generate random X and Z coordinates within the spawn area
        float randomX = Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f);
        float randomZ = Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f);

        // Return the final position (slightly above the ground)
        return new Vector3(planeCenter.x + randomX, planeCenter.y + spawnHeight, planeCenter.z + randomZ);
    }

    /// <summary>
    /// Sets a new random time for when the next power-up should spawn.
    /// </summary>
    private void SetNextSpawnTime()
    {
        nextSpawnTime = Time.time + Random.Range(minSpawnTime, maxSpawnTime);
    }

    /// <summary>
    /// Called when a power-up is collected by the player.
    /// Decreases the active power-up counter.
    /// </summary>
    private void OnPowerUpPickedUp()
    {
        currentPowerUps = Mathf.Max(0, currentPowerUps - 1);
        Debug.Log("Power-Up was collected. Current count: " + currentPowerUps);
    }

    /// <summary>
    /// Draws a yellow box in the Scene view to visualize the spawn area (only visible when the object is selected).
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = transform.position + Vector3.up * spawnHeight;
        Gizmos.DrawWireCube(center, new Vector3(spawnAreaSize.x, 0.2f, spawnAreaSize.y));
    }
}