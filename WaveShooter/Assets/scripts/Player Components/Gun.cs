using Unity.VisualScripting;
using UnityEngine;

public class Gun : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Transform rotationPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform gunPoint;
    [SerializeField] private LayerMask enemyMask;

    [Header("Attributes")]
    [SerializeField] private float targetInRange = 4f;
    [SerializeField] private float rotationspeed = 90f;
    [SerializeField] private float bulletPerSecond = 1f;
    private Transform _target;
    private float _timeUntilFire;
    private float _startXRotation;
    private float _startZRotation;


    private void Start()
    {
        _startXRotation = transform.eulerAngles.x;
        _startZRotation = transform.eulerAngles.z;
    }

    private void Update()
    {
        if (_target == null)
        {
            FindTarget();
            return;
        }
        RotateTowardsTarget();

        if (!CheckTargetIsInRange())
        {
            _target = null;
        }
        else
        {
            _timeUntilFire += Time.deltaTime;
            if (_timeUntilFire >= 1f / bulletPerSecond)
            {
                Shoot();
                _timeUntilFire = 0f;
            }

        }
    }

    private void Shoot()
    {
        Debug.Log("Turret schiet");
        GameObject bulletB = Instantiate(bulletPrefab, gunPoint.position, Quaternion.identity);
        PlayerProjectile bulletscript = bulletB.GetComponent<PlayerProjectile>();
        bulletscript.SetTarget(_target);
        Debug.Log("Pew Pew");
    }

    private void FindTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, targetInRange, enemyMask);
        if (hits.Length <= 0 )
        {
            return;
        }

        _target = hits[0].transform;

        Debug.Log("Enemy gevonden: " + hits[0].name);
    }
    private bool CheckTargetIsInRange()
    {
        float distance = Vector3.Distance(_target.position, transform.position);

        Debug.Log("Distance to target: " + distance);

        return distance <= targetInRange;

    }

    private void RotateTowardsTarget()
    {
        Vector3 direction = _target.position - transform.position;
        direction.y = 0f;

        if(direction == Vector3.zero)
        {
            return;
        }

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        float targetYRotation = lookRotation.eulerAngles.y;
        Quaternion targetRotation = Quaternion.Euler(_startXRotation, targetYRotation, _startZRotation);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationspeed * Time.deltaTime);
    }
   
}

