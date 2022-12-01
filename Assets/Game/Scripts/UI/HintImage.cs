using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintImage : MonoBehaviour
{
    public float disappearanceIntence;
    public float appearanceIntence;
    bool shownEarlier = false;
    GameObject player = null;
    CanvasGroup canvasGroup;
    private void Awake() {
        TryGetComponent(out canvasGroup);
        canvasGroup.alpha=0;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player")
        {
            //Если игрок уже видел это подсказку, то при новой игре, она не будет высвечиваться
            if (shownEarlier&&(other.gameObject!=player))
            {
                Destroy(gameObject);
                return;
            }
            if (player==null)
            {
                player=other.gameObject;
                shownEarlier = true;
            }
            StopAllCoroutines();
            StartCoroutine(Show());
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag=="Player")
        {
            StopAllCoroutines();
            StartCoroutine(Hide());
        }
    }
    IEnumerator Hide() //Исчезновение текста
    {
        yield return new WaitForSeconds(disappearanceIntence);
        if (canvasGroup.alpha >= 0f)
        {
            canvasGroup.alpha -= disappearanceIntence;
            StartCoroutine(Hide());
        }
    }
    IEnumerator Show() //Появление текста
    {

        yield return new WaitForSeconds(appearanceIntence);
        if (canvasGroup.alpha <= 1f)
        {
            canvasGroup.alpha += appearanceIntence;
            StartCoroutine(Show());
        }
    }
}
