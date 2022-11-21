using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        Character c = null;
        print("Banana");
        if (other.TryGetComponent(out c))
        {
            var hp = c.PropertyManager.GetPropertyByName("Health");
            hp.ChangeCurValue(-999);
        }
    }
}
