using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform player;

    [Header("Wave Settings")]
    [SerializeField] private float gameDuration = 300f;
    [SerializeField] private float waveIntervalStart = 10f;
    [SerializeField] private float waveIntervalMin = 3f;
    [SerializeField] private int baseEnemiesPerWave = 2;

    [Header("Difficulty Scaling")]
    [SerializeField] private float healthGrowthPerMinute = 0.20f;
    [SerializeField] private float speedGrowthPerMinute = 0.10f;
    [SerializeField] private float fireRateGrowthPerMinute = 0.10f;
    [SerializeField] private float weightGrowthPerMinute = 0.05f;

    private float _elapsedTime;
    private float _nextWaveTime;
    private bool _gameEnded;

    private void Start()
    {
        if (player == null)
        {
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }

        _nextWaveTime = waveIntervalStart;
    }

    private void Update()
    {
        if (_gameEnded) return;

        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= gameDuration)
        {
            EndGame();
            return;
        }

        if (!(_elapsedTime >= _nextWaveTime)) return;
        SpawnWave();
        _nextWaveTime = _elapsedTime + GetCurrentWaveInterval();
    }

    private void SpawnWave()
    {
        var waveNumber = Mathf.FloorToInt(_elapsedTime / 30f) + 1;
        var enemyCount = baseEnemiesPerWave + waveNumber;

        for (var i = 0; i < enemyCount; i++)
        {
            var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            var enemyObj = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

            var stats = enemyObj.GetComponent<EnemyStats>();
            var movement = enemyObj.GetComponent<EnemyMovement>();
            var shooting = enemyObj.GetComponent<EnemyShooting>();

            if (stats != null)
            {
                var minutesPassed = _elapsedTime / 60f;

                stats.maxHealth *= 1f + (healthGrowthPerMinute * minutesPassed);
                stats.moveSpeed *= 1f + (speedGrowthPerMinute * minutesPassed);
                stats.turnSpeed *= 1f + (speedGrowthPerMinute * minutesPassed);
                stats.weight *= 1f + (weightGrowthPerMinute * minutesPassed);
                stats.fireRate *= 1f + (fireRateGrowthPerMinute * minutesPassed);
            }

            if (movement != null)
            {
                movement.SetTarget(player);
            }

            if (shooting != null)
            {
                shooting.SetTarget(player);
            }
        }
    }

    private float GetCurrentWaveInterval()
    {
        var t = Mathf.Clamp01(_elapsedTime / gameDuration);
        return Mathf.Lerp(waveIntervalStart, waveIntervalMin, t);
    }

    private void EndGame()
    {
        _gameEnded = true;
        Debug.Log("Game Over - 5 minutes reached");
    }
}