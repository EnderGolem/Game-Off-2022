using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Credits : MonoBehaviour
{
    [Tooltip("Задержка между сменой текста")]
    [SerializeField] float delay;
    [Tooltip("Длительность показа текста")]
    [SerializeField] float duration;
    [Tooltip("Добавление длительности за каждый символ текста")]
    [SerializeField] float timePerSymbol;
    [Tooltip("Интенсивность появления текста")]
    [SerializeField] float appearanceIntence;
    [Tooltip("Интенсивность исчезновения текста")]
    [SerializeField] float disappearanceIntence;
    [Tooltip("Список текстовых фрагментов")]
    [TextArea(5,10)]
    [SerializeField] List<string> content = new List<string>();
    [Tooltip("Всплывающая подсказка")]
    [SerializeField] GameObject tooltip;
    TextMeshProUGUI tMesh;
    int index = 0;
    bool waitAnyKey = false;

    IEnumerator Hide() //Исчезновение текста
    {

        yield return new WaitForSeconds(disappearanceIntence);
        if (tMesh.alpha >= 0f)
        {
            tMesh.alpha -= disappearanceIntence;
            StartCoroutine(Hide());
        }
        else
        {
            yield return new WaitForSeconds(delay);
            Next();
        }
    }

    IEnumerator Show() //Появление текста
    {

        yield return new WaitForSeconds(appearanceIntence);
        if (tMesh.alpha <= 1f)
        {
            tMesh.alpha += appearanceIntence;
            StartCoroutine(Show());
        }
        else
        {
            yield return new WaitForSeconds(duration + tMesh.text.Length*timePerSymbol);
            StartCoroutine(Hide());
        }
    }

    void Next() //Следующая фраза
    {
        index++;
        if (index < content.Count)
        {
            tMesh.text = content[index];
            StartCoroutine(Show());
        }
        else
        {
            StartCoroutine(ShowTooltip());
        }
    }
    IEnumerator ShowTooltip()
    {
        yield return new WaitForSeconds(delay);
        tooltip.SetActive(true);
        waitAnyKey = true;
    }
    void BackToMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    private void Start()
    {
        Time.timeScale = 1;
        tMesh = GetComponent<TextMeshProUGUI>();
        tMesh.text = content[0];
        tMesh.alpha = 0;
        StartCoroutine(Show());
    }
    private void Update()
    {
        if (waitAnyKey&&(Input.anyKey))
        {
            BackToMainMenu();
        }
    }
}
