
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public TextMeshProUGUI levelCompletedText;

    void Start()
    {
        StartCoroutine(CheckEnemies());
    }

    IEnumerator CheckEnemies()
    {
        while (CountEnemies() == 0)
        {
            yield return new WaitForSeconds(0.25f);
        }

        while (CountEnemies() > 0)
        {
            yield return new WaitForSeconds(0.25f);
        }

        // Mostrar mensagem
        levelCompletedText.gameObject.SetActive(true);

        // Esperar 3 segundos
        yield return new WaitForSeconds(3f);

        // Carregar próxima fase
        SceneManager.LoadScene("Game2");
    }

    int CountEnemies()
    {
        int count = 0;

        GameObject[] objects = FindObjectsByType<GameObject>(
            FindObjectsSortMode.None
        );

        foreach (GameObject obj in objects)
        {
            if (obj.name.StartsWith("Enemy"))
            {
                count++;
            }
        }

        return count;
    }
}
