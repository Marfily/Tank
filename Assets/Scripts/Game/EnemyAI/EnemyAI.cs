
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public Transform turret;
    public Transform firePoint;

    [Header("Detection")]
    public float detectionRange = 12f;
    public float stoppingDistance = 5f;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float hullRotationSpeed = 90f;

    [Header("Obstacle Avoidance")]
    public float sensorLength = 3f;
    public float sensorOriginOffset = 0.8f;
    public float sensorAngle = 55f;
    public float avoidanceAngle = 60f;
    public float avoidanceDuration = 1.2f;
    public LayerMask obstacleLayer;

    private float avoidanceTimer = 0f;
    private Vector2 avoidanceTargetDirection;

    [Header("Turret")]
    public float rotationSpeed = 60f;
    public float shootingAngle = 5f;

    [Header("Shooting")]
    public float reloadTime = 3f;
    public GameObject projectilePrefab;

    private Rigidbody2D rb;
    private float nextFireTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player == null || turret == null || firePoint == null)
            return;

        float distance = Vector2.Distance(
            transform.position,
            player.position
        );

        if (distance > detectionRange)
            return;

        AimAtPlayer();

        if (distance <= stoppingDistance)
        {
            TryShoot();
        }
    }

    void FixedUpdate()
    {
        if (player == null || rb == null)
            return;

        float distance = Vector2.Distance(
            rb.position,
            player.position
        );

        if (distance > detectionRange ||
            distance <= stoppingDistance)
            return;

        Vector2 forward =
            Quaternion.Euler(0f, 0f, rb.rotation) * Vector2.up;

        Vector2 sensorOrigin =
            rb.position + forward * sensorOriginOffset;

        RaycastHit2D center = Physics2D.Raycast(
            sensorOrigin,
            forward,
            sensorLength,
            obstacleLayer
        );

        Vector2 leftDirection =
            RotateDirection(forward, sensorAngle);

        Vector2 rightDirection =
            RotateDirection(forward, -sensorAngle);

        RaycastHit2D left = Physics2D.Raycast(
            sensorOrigin,
            leftDirection,
            sensorLength,
            obstacleLayer
        );

        RaycastHit2D right = Physics2D.Raycast(
            sensorOrigin,
            rightDirection,
            sensorLength,
            obstacleLayer
        );

        avoidanceTimer -= Time.fixedDeltaTime;

        if (center.collider != null && avoidanceTimer <= 0f)
        {
            float leftClearance =
                left.collider == null ? sensorLength : left.distance;

            float rightClearance =
                right.collider == null ? sensorLength : right.distance;

            int direction;

            if (leftClearance > rightClearance)
                direction = 1;
            else if (rightClearance > leftClearance)
                direction = -1;
            else
                direction = 1;

            avoidanceTargetDirection =
                RotateDirection(forward, direction * avoidanceAngle);

            avoidanceTimer = avoidanceDuration;
        }

        Vector2 targetDirection;

        if (avoidanceTimer > 0f)
        {
            targetDirection = avoidanceTargetDirection;
        }
        else
        {
            targetDirection =
                ((Vector2)player.position - rb.position).normalized;
        }

        float targetAngle = Mathf.Atan2(
            targetDirection.y,
            targetDirection.x
        ) * Mathf.Rad2Deg - 90f;

        float newAngle = Mathf.MoveTowardsAngle(
            rb.rotation,
            targetAngle,
            hullRotationSpeed * Time.fixedDeltaTime
        );

        rb.MoveRotation(newAngle);

        Vector2 movement =
            (Vector2)(Quaternion.Euler(0f, 0f, newAngle) * Vector3.up)
            * moveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + movement);
    }

    Vector2 RotateDirection(Vector2 direction, float angle)
    {
        return Quaternion.Euler(0f, 0f, angle) * direction;
    }

    void AimAtPlayer()
    {
        Vector2 direction =
            player.position - turret.position;

        float targetAngle = Mathf.Atan2(
            direction.y,
            direction.x
        ) * Mathf.Rad2Deg - 90f;

        float newAngle = Mathf.MoveTowardsAngle(
            turret.eulerAngles.z,
            targetAngle,
            rotationSpeed * Time.deltaTime
        );

        turret.rotation = Quaternion.Euler(
            0f, 0f, newAngle
        );
    }

    void TryShoot()
    {
        if (Time.time < nextFireTime)
            return;

        Vector2 direction =
            player.position - firePoint.position;

        float angle = Vector2.Angle(
            firePoint.up,
            direction
        );

        if (angle > shootingAngle)
            return;

        GameObject shell = Instantiate(
            projectilePrefab,
            firePoint.position,
            firePoint.rotation
        );

        ProjectileMovement projectile =
            shell.GetComponent<ProjectileMovement>();

        if (projectile != null)
        {
            projectile.Initialize(transform);
        }

        nextFireTime = Time.time + reloadTime;
    }

    void OnDrawGizmosSelected()
    {
        Vector2 forward = transform.up;

        Vector2 origin =
            (Vector2)transform.position +
            forward * sensorOriginOffset;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(origin, origin + forward * sensorLength);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(
            origin,
            origin + RotateDirection(forward, sensorAngle) * sensorLength
        );

        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            origin,
            origin + RotateDirection(forward, -sensorAngle) * sensorLength
        );
    }
}
