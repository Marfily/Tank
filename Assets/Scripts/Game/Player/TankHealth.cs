
using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    public float maxHealth = 100f;

    public Slider healthSlider;

    private float currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        currentHealth = Mathf.Max(0, currentHealth);

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        Debug.Log(name + " HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            DestroyTank();
        }
    }

    void DestroyTank()
    {
        Debug.Log(name + " destroyed!");

        Destroy(gameObject);
    }
}
