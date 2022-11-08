 using System;
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    public SettingsUI settingsUI;
    public void ButtonStart()
    {
        SceneLoader.Instance.LoadScene("Loading","TilemapTest");
    }
    public void ButtonContinue()
    {
        print("Continue");
    }
    public void ButtonExit()
    {
        Application.Quit();
    }
    public void ButtonSettings()
    {
        settingsUI.Show();
    }
    public void ButtonCredits()
    {
        print("Credits");
    }
}
