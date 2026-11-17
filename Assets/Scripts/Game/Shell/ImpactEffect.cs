
using UnityEngine;
using System.Collections;

public class ImpactEffect : MonoBehaviour
{
    public float duration = 0.2f;
    public float finalScale = 1f;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(AnimateImpact());
    }

    IEnumerator AnimateImpact()
    {
        float timer = 0f;
        Color originalColor = spriteRenderer.color;

        transform.localScale = Vector3.zero;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float progress = Mathf.Clamp01(timer / duration);

            transform.localScale = Vector3.one * (progress * finalScale);

            Color newColor = originalColor;
            newColor.a = 1f - progress;
            spriteRenderer.color = newColor;

            yield return null;
        }

        Destroy(gameObject);
    }
}
