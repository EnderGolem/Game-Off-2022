using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraTarget : MonoBehaviour
{
    Vector3 startPos;
    public float maxDist;
    // Start is called before the first frame update
    void Start()
    {
        startPos=transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition)-Camera.main.transform.position;
        transform.position = -mouse;
        if (Vector2.Distance(startPos,transform.position)>maxDist)
        {
            transform.position=(Vector2)startPos+maxDist*((Vector2)transform.position-(Vector2)startPos).normalized;
        }
    }
}
