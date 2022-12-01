using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashOnScreen : MonoBehaviour
{
    public float delay=.5f;
    public float intensity=.05f;
    public float interval=.05f;
    Image rend;
    // Start is called before the first frame update
    void Awake()
    {
        rend = GetComponent<Image>();
        rend.color = new Color(rend.color.r,rend.color.g,rend.color.b,1f);
        StartCoroutine(Hide());
    }
    IEnumerator Hide()
    {
        yield return new WaitForSeconds(delay);
        while(rend.color.a>0)
        {
            yield return new WaitForSeconds(interval);
            rend.color = new Color(rend.color.r,rend.color.g,rend.color.b,rend.color.a-intensity);
        }
        Destroy(gameObject);
    }

}
