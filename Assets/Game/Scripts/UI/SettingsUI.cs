using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SettingsUI : MonoBehaviour
{
    public GameObject GraphicsSect;
    public GameObject AudioSect;

    public TMP_Dropdown screenResolution;
    public TMP_Dropdown screenMode;
    public void Show()
    {
        //������ ��� dropdown-�������� ������� ����������
        for (int i = 0; i<screenResolution.options.Count; i++)
        {
            if (screenResolution.options[i].text == $"{Screen.width} x {Screen.height}")
            {
                screenResolution.value = i;
            }
        }
        //������ ��� dropdown-�������� ������� ����� ������
        for (int i = 0; i < screenMode.options.Count; i++)
        {
            if (screenMode.options[i].text == Screen.fullScreenMode.ToString())
            {
                screenMode.value = i;
                break;
            }
        }
        gameObject.SetActive(true);
    }
    public void ButtonApply()
    {
        var resolution = screenResolution.options[screenResolution.value].text.Split(" x ");
        Screen.SetResolution(System.Int32.Parse(resolution[0]), System.Int32.Parse(resolution[1]), Screen.fullScreenMode);
        switch (screenMode.value)
        {
            case 0: Screen.fullScreenMode = FullScreenMode.FullScreenWindow; break;
            case 1: Screen.fullScreenMode = FullScreenMode.MaximizedWindow; break;
            case 2: Screen.fullScreenMode = FullScreenMode.Windowed; break;
        }
    }
    public void ButtonCancel()
    {
        gameObject.SetActive(false);
    }
    public void ButtonOk()
    {
        ButtonApply();
        ButtonCancel();
    }
    public void ButtonGraphics()
    {
        AudioSect.SetActive(false);
        GraphicsSect.SetActive(true);
    }
    public void ButtonAudio()
    {
        AudioSect.SetActive(true);
        GraphicsSect.SetActive(false);
    }
}
