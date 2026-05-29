using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [Header("Power-Up Settings")]
    [SerializeField] private GameObject _powerUpPrefab;
    [SerializeField] private int _maxPowerUpsAtOnce = 3;

    [Header("Spawn Timing")]
    [SerializeField] private float _minSpawnTime = 8f;
    [SerializeField] private float _maxSpawnTime = 15f;

    [Header("Spawn Area")]
    [SerializeField] private float _spawnHeight = 1f;
    [SerializeField] private Vector2 _spawnAreaSize = new Vector2(40f, 40f);

    private int _currentPowerUps = 0;
    private float _nextSpawnTime;

    private void Start()
    {
        SetNextSpawnTime();
    }

    private void Update()
    {
        if (Time.time >= _nextSpawnTime)
        {
            TrySpawnPowerUp();
        }
    }

    private void TrySpawnPowerUp()
    {
        if (_powerUpPrefab == null) return;
        if (_currentPowerUps >= _maxPowerUpsAtOnce) 
        {
            SetNextSpawnTime();
            return;
        }

        SpawnPowerUp();
        SetNextSpawnTime();
    }

    private void SpawnPowerUp()
    {
        Vector3 randomPosition = GetRandomSpawnPosition();

        GameObject powerUp = Instantiate(_powerUpPrefab, randomPosition, Quaternion.identity);
        powerUp.transform.parent = transform;

        _currentPowerUps++;

        Debug.Log($"Power-Up spawned at: {randomPosition}");

        // Subscribe to the pickup event
        PowerUpPickup pickup = powerUp.GetComponent<PowerUpPickup>();
        if (pickup != null)
        {
            pickup.OnPickedUpEvent.AddListener(OnPowerUpPickedUp);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 planeCenter = transform.position;

        float randomX = Random.Range(-_spawnAreaSize.x / 2f, _spawnAreaSize.x / 2f);
        float randomZ = Random.Range(-_spawnAreaSize.y / 2f, _spawnAreaSize.y / 2f);

        return new Vector3(planeCenter.x + randomX, planeCenter.y + _spawnHeight, planeCenter.z + randomZ);
    }

    private void SetNextSpawnTime()
    {
        _nextSpawnTime = Time.time + Random.Range(_minSpawnTime, _maxSpawnTime);
    }

    /// <summary>
    /// Called when a power-up is collected
    /// </summary>
    private void OnPowerUpPickedUp()
    {
        _currentPowerUps = Mathf.Max(0, _currentPowerUps - 1);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = transform.position + Vector3.up * _spawnHeight;
        Gizmos.DrawWireCube(center, new Vector3(_spawnAreaSize.x, 0.1f, _spawnAreaSize.y));
    }
}