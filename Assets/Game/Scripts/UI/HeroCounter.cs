using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HeroCounter : MonoBehaviour
{
    TextMeshProUGUI tMesh;
    int count = 2;

    private void Awake() {
        TryGetComponent(out tMesh);
    }
    
    public void Inc()
    {
        count++;
        tMesh.text = $"HERO №{count}";
    }
}
