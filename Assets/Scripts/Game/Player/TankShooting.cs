
using UnityEngine;
using System.Collections;

public class TankShooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public GameObject muzzleFlash;

    public float reloadTime = 2f;
    public float flashDuration = 0.08f;

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

        StartCoroutine(ShowMuzzleFlash());
    }

    IEnumerator ShowMuzzleFlash()
    {
        if (muzzleFlash == null)
            yield break;

        muzzleFlash.SetActive(true);

        yield return new WaitForSeconds(flashDuration);

        muzzleFlash.SetActive(false);
    }
}
