using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private Transform target;

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 9f;
    [SerializeField] private int bulletDamage = 1;
    [SerializeField] private float hitDistance = 0.5f;
    [Header("References")]
    [SerializeField] private Rigidbody rb;


    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        var toTarget = target.position - transform.position;
        var distance = toTarget.magnitude;

        if (distance <= hitDistance)
        {
            HitTarget();
            return;
        }

        var dir = toTarget.normalized;
        transform.position += dir * (bulletSpeed * Time.deltaTime);

        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }
    } 

    private void HitTarget() {
        var health = target.GetComponent<EnemyStats>();
        if (health != null)
        {
            Debug.Log("Hit");
            health.TakeDamage(bulletDamage);
        }
        Destroy(gameObject);
    }
}

