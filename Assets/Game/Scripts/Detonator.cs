using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detonator : MonoBehaviour
{
    public List<Killer> killers = new List<Killer>();
    public float delay = .5f;

    void Awake()
    {
        StartCoroutine(Detonate());
    }

    IEnumerator Detonate()
    {
        foreach(var obj in killers)
        {
            yield return new WaitForSeconds(delay);
            obj.Kill();
        }
        Destroy(gameObject);
    }
}
