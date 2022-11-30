using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Overlay : MonoBehaviour
{
    [HideInInspector] public Character player;
    PauseGame pauseGame;
    RespawnManager respawnManager;
    public BloodScreen bloodScreen;
    public BoltsCounter boltsCounter;

    private void Awake()
    {
        respawnManager = GetComponent<RespawnManager>();
        pauseGame = GetComponent<PauseGame>();
        respawnManager.owner = this;
        pauseGame.owner = this;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        pauseGame.Initialize();
        respawnManager.Initialize();
        bloodScreen.SetOwner(player);
        boltsCounter.SetOwner(player);
        pauseGame.playerInput = player.GetComponent<PlayerInput>();
        var bars = GetComponentsInChildren<PropertyProgressBar>();
        foreach (var x in bars)
        {
            x.SetOwner(player);
        }
    }
}
