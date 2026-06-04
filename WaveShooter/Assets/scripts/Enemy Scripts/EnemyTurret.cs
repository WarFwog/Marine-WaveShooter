using UnityEngine;

public class EnemyTurret : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private EnemyProjectile projectilePrefab;

    private EnemyStats _stats;
    private Transform _target;

    private float _timeUntilNextFireTime;

    private void Awake()
    {
        _stats = GetComponentInParent<EnemyStats>();
    }
    
    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void Update()
    {
        if (_target == null) return;

        var distance = Vector3.Distance(transform.root.position, _target.position);

        if (distance > _stats.shootingRange) return;
        
        AimAtTarget();

        if (!(Time.time >= _timeUntilNextFireTime)) return; 
        
        Shoot();
        _timeUntilNextFireTime = Time.time + (1f / _stats.fireRate);
    }

    private void AimAtTarget()
    {
        var direction = _target.position - transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0f, 0f, angle);
    }
    
    private void Shoot()
    {
        if (projectilePrefab == null || firePoint == null) return;
        
        var projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        projectile.Init(_target, _stats.damage);
    }
}