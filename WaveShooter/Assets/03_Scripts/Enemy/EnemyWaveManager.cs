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

    private float elapsedTime;
    private float nextWaveTime;
    private bool gameEnded;

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }

        nextWaveTime = startSpawnInterval;
    }

    private void Update()
    {
        if (gameEnded)
            return;

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= gameDuration)
        {
            EndGame();
            return;
        }

        if (elapsedTime >= nextWaveTime)
        {
            SpawnWave();

            nextWaveTime = elapsedTime + GetSpawnInterval();
        }
    }

    private void SpawnWave()
    {
        int enemyCount = GetEnemyCount();

        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (enemyTypes.Length == 0 || spawnPoints.Length == 0)
            return;

        EnemyType selectedEnemy = GetRandomEnemyType();

        Transform spawnPoint =
            spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject enemyObj =
            Instantiate(
                selectedEnemy.prefab,
                spawnPoint.position,
                spawnPoint.rotation);

        EnemyStats stats = enemyObj.GetComponent<EnemyStats>();

        if (stats != null)
        {
            ApplyDifficultyScaling(stats);
        }

        EnemyMovement movement =
            enemyObj.GetComponent<EnemyMovement>();

        if (movement != null)
        {
            movement.SetTarget(player);
        }

        EnemyShooting shooting =
            enemyObj.GetComponent<EnemyShooting>();

        if (shooting != null)
        {
            shooting.SetTarget(player);
        }
    }

    private EnemyType GetRandomEnemyType()
    {
        float totalWeight = 0f;

        foreach (EnemyType enemy in enemyTypes)
        {
            totalWeight += enemy.spawnWeight;
        }

        float randomValue = Random.Range(0f, totalWeight);

        float currentWeight = 0f;

        foreach (EnemyType enemy in enemyTypes)
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
        float minutesPassed = elapsedTime / 60f;

        stats.maxHealth *=
            1f + (healthMultiplierPerMinute * minutesPassed);

        stats.moveSpeed *=
            1f + (speedMultiplierPerMinute * minutesPassed);

        stats.fireRate *=
            1f + (fireRateMultiplierPerMinute * minutesPassed);
    }

    private int GetEnemyCount()
    {
        float progress = elapsedTime / gameDuration;

        return Mathf.RoundToInt(
            Mathf.Lerp(
                startEnemiesPerWave,
                maxEnemiesPerWave,
                progress));
    }

    private float GetSpawnInterval()
    {
        float progress = elapsedTime / gameDuration;

        return Mathf.Lerp(
            startSpawnInterval,
            minimumSpawnInterval,
            progress);
    }

    private void EndGame()
    {
        gameEnded = true;

        Debug.Log("5 minutes survived!");
    }
}