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

    private void Update()
    {
        if (target == null) return;

        var toTarget = target.position - transform.position;
        toTarget.y = 0f;

        if (toTarget.sqrMagnitude < 0.01f) return;

        var weight = Mathf.Max(0.1f, _stats.weight);
        transform.rotation = CalculateRotation(toTarget, weight);

        CalculateVelocity(weight);
        transform.position += _velocity * Time.deltaTime;
    }
    private Quaternion CalculateRotation(Vector3 toTarget, float weight)
    {
        var rotation = Quaternion.LookRotation(toTarget.normalized, Vector3.up);
        var turnAmount = (_stats.turnSpeed / weight) * Time.deltaTime;
        return Quaternion.RotateTowards(transform.rotation, rotation, turnAmount);
    }

    private void CalculateVelocity(float weight)
    {
        var desiredSpeed = _stats.moveSpeed / weight;
        var desiredVelocity = transform.forward * desiredSpeed;

        var acceleration = _stats.acceleration / weight;
        _velocity = Vector3.MoveTowards(_velocity, desiredVelocity, acceleration * Time.deltaTime);
    }
    
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}