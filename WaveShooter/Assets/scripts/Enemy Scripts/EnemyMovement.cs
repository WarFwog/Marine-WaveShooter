using UnityEngine;

[RequireComponent(typeof(EnemyStats))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform target;

    private EnemyStats _stats;
    private Vector3 _velocity;

    private void Awake()
    {
        _stats = GetComponent<EnemyStats>();
    }

    private void Start()
    {
        if (target != null) return;
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.transform;
        }
    }

    private void Update()
    {
        if (target == null) return;

        var toTarget = target.position - transform.position;
        toTarget.y = 0f;

        if (toTarget.sqrMagnitude < 0.01f) return;

        var weight = Mathf.Max(0.1f, _stats.weight);
        
        var rotation = Quaternion.LookRotation(toTarget.normalized, Vector3.up);
        var turnAmount = (_stats.turnSpeed / weight) * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, turnAmount);
        
        var desiredSpeed = _stats.moveSpeed / weight;
        var desiredVelocity = transform.forward * desiredSpeed;

        var acceleration = _stats.acceleration / weight;
        _velocity = Vector3.MoveTowards(_velocity, desiredVelocity, acceleration * Time.deltaTime);

        transform.position += _velocity * Time.deltaTime;
    }

    //voor als we willen toevoegen dat de player distractions kan neerzetten ofzo
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}