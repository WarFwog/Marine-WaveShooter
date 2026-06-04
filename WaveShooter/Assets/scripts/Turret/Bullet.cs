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
        if (target == null)
        
            {
                Destroy(gameObject);
                return;
            }

            Vector3 direction = (target.position - transform.position).normalized;
            rb.linearVelocity = direction * bulletSpeed;
        } 

        private void OnCollisionEnter(Collision collision) {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null) 
        {
            enemy.TakeDamage(bulletDamage);
        }

        Destroy(gameObject);
    }
}

