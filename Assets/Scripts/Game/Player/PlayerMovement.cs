
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 120f;

    private Rigidbody2D rb;

    private float moveInput;
    private float turnInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Vertical");
        turnInput = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        // Rotate the tank
        float rotation = -turnInput * rotationSpeed * Time.fixedDeltaTime;

        rb.MoveRotation(rb.rotation + rotation);

        // Move forward or backward
        rb.linearVelocity = (Vector2)transform.up * moveInput * moveSpeed;
    }
}
