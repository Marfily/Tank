
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TankHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public Slider healthSlider;

    [Header("Player Respawn")]
    public bool isPlayer = false;
    public Transform playerSpawn;
    public float respawnTime = 2f;

    private float currentHealth;
    private bool isDead = false;

    private SpriteRenderer[] sprites;
    private Collider2D[] colliders;
    private Rigidbody2D rb;

    void Awake()
    {
        currentHealth = maxHealth;

        sprites = GetComponentsInChildren<SpriteRenderer>();
        colliders = GetComponentsInChildren<Collider2D>();
        rb = GetComponent<Rigidbody2D>();

        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);

        UpdateHealthBar();

        Debug.Log(name + " HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            DestroyTank();
        }
    }

    void UpdateHealthBar()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    void DestroyTank()
    {
        Debug.Log(name + " destroyed!");

        if (isPlayer)
        {
            isDead = true;
            StartCoroutine(Respawn());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator Respawn()
    {
        // Esconder o tanque
        foreach (SpriteRenderer sprite in sprites)
            sprite.enabled = false;

        foreach (Collider2D col in colliders)
            col.enabled = false;

        if (rb != null)
            rb.simulated = false;

        // Esperar antes de reaparecer
        yield return new WaitForSeconds(respawnTime);

        // Reposicionar o tanque
        transform.SetPositionAndRotation(
            playerSpawn.position,
            playerSpawn.rotation
        );

        // Restaurar velocidade
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = true;
        }

        // Restaurar vida
        currentHealth = maxHealth;
        UpdateHealthBar();

        // Mostrar novamente
        foreach (SpriteRenderer sprite in sprites)
            sprite.enabled = true;

        foreach (Collider2D col in colliders)
            col.enabled = true;

        isDead = false;

        Debug.Log("Player respawned!");
    }
}
