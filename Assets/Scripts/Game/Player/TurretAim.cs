
using UnityEngine;

public class TurretAim : MonoBehaviour
{
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

        float angle = Mathf.Atan2(
            aimDirection.y,
            aimDirection.x
        ) * Mathf.Rad2Deg - 90f;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
