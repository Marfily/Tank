
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

        Vector2 direction =
            (Vector2)player.position - rb.position;

        float targetAngle = Mathf.Atan2(
            direction.y,
            direction.x
        ) * Mathf.Rad2Deg - 90f;

        float newAngle = Mathf.MoveTowardsAngle(
            rb.rotation,
            targetAngle,
            hullRotationSpeed * Time.fixedDeltaTime
        );

        rb.MoveRotation(newAngle);

        Vector2 movement =
            (Vector2)transform.up *
            moveSpeed *
            Time.fixedDeltaTime;

        rb.MovePosition(rb.position + movement);
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
}
