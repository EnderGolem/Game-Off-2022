using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform portalPoint;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag=="Player")
        {
            other.transform.position = portalPoint.position; 
        }
    }
}
