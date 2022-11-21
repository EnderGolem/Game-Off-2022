using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overlay : MonoBehaviour
{
    [HideInInspector] public Character player;
    PauseGame pauseGame;
    RespawnManager respawnManager;

    private void Awake()
    {
        respawnManager = GetComponent<RespawnManager>();
        pauseGame = GetComponent<PauseGame>();
        respawnManager.owner = this;
        pauseGame.owner = this;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        pauseGame.Initialize();
        respawnManager.Initialize();
        var bars = GetComponentsInChildren<PropertyProgressBar>();
        foreach (var x in bars)
        {
            x.SetOwner(player);
        }
    }
}
