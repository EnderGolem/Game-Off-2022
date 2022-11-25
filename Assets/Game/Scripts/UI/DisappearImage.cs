using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearImage : MonoBehaviour
{
    public float disappearanceIntence;
    public float speed;
    CanvasGroup canvasGroup;
    private void Awake() {
        TryGetComponent(out canvasGroup);
        StartCoroutine(Hide());
    }
    IEnumerator Hide() //Исчезновение текста
    {
        yield return new WaitForSeconds(disappearanceIntence);
        if (canvasGroup.alpha >= 0f)
        {
            canvasGroup.alpha -= disappearanceIntence;
            StartCoroutine(Hide());
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        transform.Translate(0,speed*Time.deltaTime,0);
    }
}
