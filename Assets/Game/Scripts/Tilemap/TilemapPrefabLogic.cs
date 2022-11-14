using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapPrefabLogic : MonoBehaviour
{
    private void Awake()
    {
        var mapRenderer = GetComponent<TilemapRenderer>();
        mapRenderer.enabled = false;
    }
}