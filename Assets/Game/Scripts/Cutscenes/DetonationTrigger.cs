using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetonationTrigger : MonoBehaviour
{
    public GameObject detonationTimeline;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag=="Player")
        {
            gameObject.SetActive(false);
            detonationTimeline.SetActive(true);
        }
    }
}
