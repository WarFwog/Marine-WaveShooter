using UnityEngine;
using UnityEngine.InputSystem;

public class GunshipShooting : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _fireRate = 0.15f;
    [SerializeField] private LayerMask _targetLayer;

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
        if (GunshipCamera.Instance == null) return;

        _nextFireTime = Time.time + _fireRate;

        Shoot();
    }

    private void Shoot()
    {
        if (GunshipCamera.Instance.GetMouseWorldPosition(out Vector3 hitPoint, _targetLayer))
        {
            AimAtTarget(hitPoint);
            PlayMuzzleFlash();
            FireProjectile(hitPoint);
        }
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

    private void FireProjectile(Vector3 targetPosition)
    {
        if (_projectilePrefab == null) return;

        Vector3 spawnPosition = _firePoint != null ? _firePoint.position : transform.position;
        Quaternion direction = Quaternion.LookRotation(targetPosition - spawnPosition);

        GameObject projectile = Instantiate(_projectilePrefab, spawnPosition, direction);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = projectile.transform.forward * 80f;
        }
    }
}