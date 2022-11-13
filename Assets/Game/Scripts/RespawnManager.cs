using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RespawnManager : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject playerPref;
    [SerializeField] Character player;

    [SerializeField] GameObject DeadScreen;
    [SerializeField] PropertyProgressBar healthBar;
    [SerializeField] PropertyProgressBar enduranceBar;
    [SerializeField] EscMenuUI escMenu;
    bool canRespawn = false;
    private void Start()
    {
        escMenu.OnCryCravenButtonPressed.AddListener(CryCraven);
        player?.OnDead.AddListener(OnPlayerDead);
        if (spawnPoint == null && player != null)
        {
            spawnPoint = new GameObject("PlayerRespawnPoint").transform;
            spawnPoint.position = player.transform.position;
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
    private void OnPlayerDead()
    {
        canRespawn = true;
        DeadScreen.SetActive(true);
    }
    private void CryCraven()
    {
        if (player != null)
        {
            player.PropertyManager.GetPropertyByName("Health").ChangeCurValue(-100);
        }
    }
    public void Respawn()
    {
        if (player==null)
        {
            canRespawn = false;
            player = Instantiate(playerPref, spawnPoint.position,spawnPoint.rotation).GetComponent<Character>();
            player.gameObject.name = "Player";
            player.OnDead.AddListener(OnPlayerDead);
            healthBar.SetOwner(player);
            enduranceBar.SetOwner(player);
            DeadScreen.SetActive(false);
            var cam = Camera.main.GetComponent<CameraTracking>();
            cam.SetTrackingObject(player.cameraTarget);
            cam.SetZoom(5);
            Time.timeScale = 1f;
        }
    }
}
