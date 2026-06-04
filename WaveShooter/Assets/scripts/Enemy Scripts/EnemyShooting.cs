using UnityEngine;

[RequireComponent(typeof(EnemyStats))]
public class EnemyShooting : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform firePoint;
    [SerializeField] private EnemyProjectile projectilePrefab;

    private EnemyStats _stats;
    private float _nextFireTime;

    private void Awake()
    {
        _stats = GetComponent<EnemyStats>();
    }

    private void Start()
    {
        if (target != null) return;
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.transform;
        }
    }

    private void Update()
    {
        if (target == null || firePoint == null || projectilePrefab == null) return;

        var distance = Vector3.Distance(transform.position, target.position);

        if (!(distance <= _stats.shootingRange) || !(Time.time >= _nextFireTime)) return;
        Shoot();
        _nextFireTime = Time.time + (1f / Mathf.Max(0.1f, _stats.fireRate));
    }

    private void Shoot()
    {
        var projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        projectile.Init(target, _stats.damage);
    }

    //voor als we willen toevoegen dat de player distractions kan neerzetten ofzo
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}