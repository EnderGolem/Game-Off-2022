using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RespawnManager : MonoBehaviour
{
    [SerializeField] Vector3 spawnPoint;
    [SerializeField] GameObject playerPref;
    [HideInInspector] public Overlay owner;

    [SerializeField] GameObject DeadScreen;
    [SerializeField] PropertyProgressBar healthBar;
    [SerializeField] PropertyProgressBar enduranceBar;
    [SerializeField] EscMenuUI escMenu;
    bool canRespawn = false;
    public void Initialize()
    {
        escMenu.OnCryCravenButtonPressed.AddListener(CryCraven);
        owner.player?.OnDead.AddListener(OnPlayerDead);
        if (owner.player != null)
        {
            spawnPoint = owner.player.transform.position;
        }
    }
    public void KeyPressed(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (canRespawn)
            {
                Respawn();
            }
        }
    }
    private void OnPlayerDead(Character character = null)
    {
        canRespawn = true;
        DeadScreen.SetActive(true);
    }
    private void CryCraven()
    {
        if (owner.player != null)
        {
            owner.player.PropertyManager.GetPropertyByName("Health").ChangeCurValue(-100);
        }
    }
    public void Respawn()
    {
        if (owner.player == null)
        {
            canRespawn = false;
            owner.player = Instantiate(playerPref, spawnPoint,new Quaternion(0,0,0,0)).GetComponent<Character>();
            owner.player.gameObject.name = "Player";
            owner.player.OnDead.AddListener(OnPlayerDead);
            healthBar.SetOwner(owner.player);
            enduranceBar.SetOwner(owner.player);
            DeadScreen.SetActive(false);
            var cam = Camera.main.GetComponent<CameraTracking>();
            cam.SetTrackingObject(owner.player.cameraTarget);
            cam.SetZoom(5);
            Time.timeScale = 1f;
        }
    }
}
