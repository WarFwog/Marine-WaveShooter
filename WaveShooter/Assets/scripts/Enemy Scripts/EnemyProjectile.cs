using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 25f;
    [SerializeField] private float hitDistance = 0.5f;
    [SerializeField] private float lifeTime = 10f;

    private Transform _target;
    private float _damage;

    public void Init(Transform newTarget, float newDamage)
    {
        _target = newTarget;
        _damage = newDamage;
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        if (_target == null)
        {
            Destroy(gameObject);
            return;
        }

        var toTarget = _target.position - transform.position;
        var distance = toTarget.magnitude;

        if (distance <= hitDistance)
        {
            HitTarget();
            return;
        }

        var dir = toTarget.normalized;
        transform.position += dir * (speed * Time.deltaTime);

        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }
    }

    private void HitTarget()
    {
        var health = _target.GetComponent<PlayerStats>();
        if (health != null)
        {
            health.TakeDamage(_damage);
        }
        Destroy(gameObject);
    }
}