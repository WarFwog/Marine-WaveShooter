using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] Transform target;

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 9f;
    [SerializeField] private int bulletDamage = 1;
    [Header("References")]
    [SerializeField] private Rigidbody rb;


    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void FixedUpdate()
    {
        if (!target) return;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * bulletSpeed;
    }
 
    private void OnCollisionEnter2D(Collision2D other)
    {
        var health = target.GetComponent<EnemyStats>();
        if (health != null)
        {
            Debug.Log("Hit");
            health.TakeDamage(bulletDamage);
        }
        Destroy(gameObject);
    }
}
