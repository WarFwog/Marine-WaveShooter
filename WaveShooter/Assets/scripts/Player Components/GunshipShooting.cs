using UnityEngine;
using UnityEngine.InputSystem;

public class GunshipShooting : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _fireRate = 0.15f;

    [Header("Particle Effects")]
    [SerializeField] private ParticleSystem _muzzleFlashParticles;

    private InputAction _shootAction;
    private float _nextFireTime;

    private void Awake()
    {
        _shootAction = new InputAction("Shoot", binding: "<Pointer>/press");

        _shootAction.Enable();
        _shootAction.performed += OnShootPerformed;
    }

    private void OnDestroy()
    {
        _shootAction.performed -= OnShootPerformed;
        _shootAction.Disable();
    }

    private void OnShootPerformed(InputAction.CallbackContext context)
    {
        if (Time.time < _nextFireTime) return;

        _nextFireTime = Time.time + _fireRate;
        Shoot();
    }

    private void Shoot()
    {
        var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, 1000f)) return;
        var enemy = hit.collider.GetComponentInParent<EnemyStats>();

        if (enemy == null)
        {
            Debug.Log("No enemy under crosshair.");
            return;
        }

        AimAtTarget(enemy.transform.position);
        PlayMuzzleFlash();
        FireProjectile(enemy.transform);
    }

    private void AimAtTarget(Vector3 targetPosition)
    {
        if (_firePoint != null)
        {
            _firePoint.LookAt(targetPosition);
        }
    }

    private void PlayMuzzleFlash()
    {
        if (_muzzleFlashParticles != null)
        {
            _muzzleFlashParticles.Play();
        }
    }

    private void FireProjectile(Transform target)
    {
        if (_projectilePrefab == null) return;

        var spawnPosition = _firePoint != null ? _firePoint.position : transform.position;

        var projectileObj = Instantiate(_projectilePrefab, spawnPosition, Quaternion.identity);

        var projectile = projectileObj.GetComponent<PlayerProjectile>();

        if (projectile != null)
        {
            projectile.SetTarget(target);
        }
    }
}