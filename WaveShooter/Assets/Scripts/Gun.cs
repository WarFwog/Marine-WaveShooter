using UnityEngine;

public class Gun : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Transform Rotationpoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform gunpoint;
    [SerializeField] private LayerMask enemyMask;

    [Header("Attributes")]
    [SerializeField] private float targetInRange = 4f;
    [SerializeField] private float rotationspeed = 5f;
    [SerializeField] private float bulletPerSecond = 1f;
    [SerializeField] private Transform target;
    [SerializeField] private float timeUntilFire;

    private void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }
        RotateTowardsTarget();

        if (!CheckTargetIsInRange())
        {
            target = null;
        }
        else
        {
            timeUntilFire += Time.deltaTime;
            if (timeUntilFire >= 1f / bulletPerSecond)
            {
                Shoot();
                timeUntilFire = 0f;
            }

        }
    }

    private void Shoot()
    {
        GameObject bulletB = Instantiate(bulletPrefab, gunpoint.position, Quaternion.identity);
        Bullet bulletscript = bulletB.GetComponent<Bullet>();
        bulletscript.SetTarget(target);
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetInRange, (Vector2)
            transform.position, 0f, enemyMask);
        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }
    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetInRange;
    }
    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        Rotationpoint.rotation = Quaternion.RotateTowards(Rotationpoint.rotation, targetRotation, rotationspeed * Time.deltaTime);
    }
}
