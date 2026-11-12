
using UnityEngine;

public class TankShooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;

    public float reloadTime = 2f;

    private float nextFireTime = 0f;

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();

            nextFireTime = Time.time + reloadTime;
        }
    }

    void Shoot()
    {
        GameObject shell = Instantiate(
            projectilePrefab,
            firePoint.position,
            firePoint.rotation
        );

        ProjectileMovement projectile =
            shell.GetComponent<ProjectileMovement>();

        if (projectile != null)
        {
            projectile.Initialize(transform.root);
        }
    }
}
