
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    public float projectileSpeed = 15f;
    public float lifetime = 5f;

    private Rigidbody2D rb;
    private Transform owner;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Transform shooter)
    {
        owner = shooter;

        rb.linearVelocity = (Vector2)transform.up * projectileSpeed;
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root == owner)
            return;

        Destroy(gameObject);
    }
}
