using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EscMenuUI : MonoBehaviour
{
    public SettingsUI settingsUI;

    [HideInInspector] public UnityEvent OnContinueButtonPressed;
    [HideInInspector] public UnityEvent OnCryCravenButtonPressed;

    float fixedDeltaTimeDefault;

    private void Awake() {
        fixedDeltaTimeDefault = Time.fixedDeltaTime;
    }
    
    public bool Close()
    {
        if (!settingsUI.gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
            return true;
        }
        else
        {
            settingsUI.ButtonCancel();
            return false;
        }
    }
    public bool Open()
    {
        gameObject.SetActive(true);
        return true;
    }
    public void ButtonContinue()
    {
        OnContinueButtonPressed.Invoke();
    }
    public void ButtonSettings()
    {
        settingsUI.Show();
    }
    public void ButtonExitToMainMenu()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = fixedDeltaTimeDefault;
        SceneManager.LoadScene("MenuScene");
    }
    public void ButtonCryCraven()
    {
        OnCryCravenButtonPressed.Invoke();
    }
}
