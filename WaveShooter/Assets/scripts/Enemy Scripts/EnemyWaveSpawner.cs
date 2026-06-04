using System.Linq;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public string enemyName;
        public GameObject prefab;

        [Range(0f, 1f)]
        public float spawnWeight = 1f;
    }

    [Header("Enemy Types")]
    public EnemyType[] enemyTypes;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Player")]
    [SerializeField] private Transform player;

    [Header("Game Settings")]
    [SerializeField] private float gameDuration = 300f;

    [Header("Wave Settings")]
    [SerializeField] private float startSpawnInterval = 4f;
    [SerializeField] private float minimumSpawnInterval = 1f;

    [SerializeField] private int startEnemiesPerWave = 2;
    [SerializeField] private int maxEnemiesPerWave = 15;

    [Header("Difficulty Scaling")]
    [SerializeField] private float healthMultiplierPerMinute = 0.25f;
    [SerializeField] private float speedMultiplierPerMinute = 0.08f;
    [SerializeField] private float fireRateMultiplierPerMinute = 0.10f;

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
        _nextWaveTime = startSpawnInterval;
    }

    private void Update()
    {
        if (_gameEnded)
            return;

        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= gameDuration)
        {
            EndGame();
            return;
        }

        if (!(_elapsedTime >= _nextWaveTime)) return;
        SpawnWave();

        _nextWaveTime = _elapsedTime + GetSpawnInterval();
    }

    private void SpawnWave()
    {
        var enemyCount = GetEnemyCount();

        for (var i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (enemyTypes.Length == 0 || spawnPoints.Length == 0)
            return;

        var selectedEnemy = GetRandomEnemyType();
        var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        var enemyObj = Instantiate(selectedEnemy.prefab, spawnPoint.position, spawnPoint.rotation);

        var stats = enemyObj.GetComponent<EnemyStats>();
        if (stats != null)
        {
            ApplyDifficultyScaling(stats);
        }

        var movement = enemyObj.GetComponent<EnemyMovement>();
        if (movement != null)
        {
            movement.SetTarget(player);
        }

        var shooting = enemyObj.GetComponent<EnemyShooting>();
        if (shooting != null)
        {
            shooting.SetTarget(player);
        }
    }

    private EnemyType GetRandomEnemyType()
    {
        var totalWeight = enemyTypes.Sum(enemy => enemy.spawnWeight);
        var randomValue = Random.Range(0f, totalWeight);
        var currentWeight = 0f;

        foreach (var enemy in enemyTypes)
        {
            currentWeight += enemy.spawnWeight;
            if (randomValue <= currentWeight)
            {
                return enemy;
            }
        }
        return enemyTypes[0];
    }

    private void ApplyDifficultyScaling(EnemyStats stats)
    {
        var minutesPassed = _elapsedTime / 60f;

        stats.maxHealth *= 1f + (healthMultiplierPerMinute * minutesPassed);
        stats.moveSpeed *= 1f + (speedMultiplierPerMinute * minutesPassed);
        stats.fireRate *= 1f + (fireRateMultiplierPerMinute * minutesPassed);
    }

    private int GetEnemyCount()
    {
        var progress = _elapsedTime / gameDuration;

        return Mathf.RoundToInt(Mathf.Lerp(startEnemiesPerWave, maxEnemiesPerWave, progress));
    }

    private float GetSpawnInterval()
    {
        var progress = _elapsedTime / gameDuration;

        return Mathf.Lerp(startSpawnInterval, minimumSpawnInterval, progress);
    }

    private void EndGame()
    {
        _gameEnded = true;
        
        Debug.Log("5 minutes survived!"); //vervangen voor win method
    }
}