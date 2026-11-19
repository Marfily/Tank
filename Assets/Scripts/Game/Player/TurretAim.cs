
using UnityEngine;

public class TurretAim : MonoBehaviour
{
    public float rotationSpeed = 90f;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -mainCamera.transform.position.z;

        Vector3 mouseWorldPosition =
            mainCamera.ScreenToWorldPoint(mousePosition);

        Vector2 aimDirection =
            mouseWorldPosition - transform.position;

        float targetAngle = Mathf.Atan2(
            aimDirection.y,
            aimDirection.x
        ) * Mathf.Rad2Deg - 90f;

        float newAngle = Mathf.MoveTowardsAngle(
            transform.eulerAngles.z,
            targetAngle,
            rotationSpeed * Time.deltaTime
        );

        transform.rotation = Quaternion.Euler(0f, 0f, newAngle);
    }
}
