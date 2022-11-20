using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteTracker : MonoBehaviour
{
    public IEnumerator<Transform> pointInRoute;

    public bool moving = false;
    public float speed = 0.1f;


    private void Update() {
        if (moving&&pointInRoute.Current!=null)
        {
            transform.position += (pointInRoute.Current.position - transform.position).normalized * speed * Time.deltaTime;
        }
    }
}
