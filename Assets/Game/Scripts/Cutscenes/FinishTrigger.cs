using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FinishTrigger : MonoBehaviour
{
    public GameObject outro;
    public GameObject overlay;
    public GameObject outroOverlay;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag=="Player")
        {
            other.GetComponent<PlayerInput>().enabled=false;
            overlay.SetActive(false);
            outro.SetActive(true);
            outroOverlay.SetActive(true);
        }
    }
}
