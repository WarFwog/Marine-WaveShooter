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
        // === TEST LOG: Check if we successfully got a world position from the mouse ===
        bool hitSuccess = GunshipCamera.Instance.GetMouseWorldPosition(out Vector3 hitPoint, _targetLayer);

        if (hitSuccess)
        {
            Debug.Log($"Raycast SUCCESS - Hit world position: {hitPoint}");

            AimAtTarget(hitPoint);
            PlayMuzzleFlash();
            FireProjectile(hitPoint);
        }
        else
        {
            Debug.Log($"Raycast FAILED - No hit detected. Using fallback position.");
            
            // Still try to shoot even if no physics hit
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
            Debug.Log($"Gun aimed at position: {targetPosition}");
        }
    }

    private void PlayMuzzleFlash()
    {
        if (_muzzleFlashParticles != null)
        {
            _muzzleFlashParticles.Play();
            Debug.Log("Muzzle flash played");
        }
    }

    private void FireProjectile(Vector3 targetPosition)
    {
        if (_projectilePrefab == null) 
        {
            Debug.LogWarning("No projectile prefab assigned!");
            return;
        }

        Vector3 spawnPosition = _firePoint != null ? _firePoint.position : transform.position;
        Quaternion direction = Quaternion.LookRotation(targetPosition - spawnPosition);

        GameObject projectile = Instantiate(_projectilePrefab, spawnPosition, direction);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = projectile.transform.forward * 80f;
            Debug.Log($"Projectile fired towards {targetPosition}");
        }
    }
}