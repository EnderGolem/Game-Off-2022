using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Cinemachine.PostFX;
using UnityEngine;

public class BloodScreen : MonoBehaviour
{
    CinemachineVirtualCamera vcam;
    CinemachineVolumeSettings fx_settings;
    UnityEngine.Rendering.Universal.Vignette vignette;
	protected Character owner;
    protected ObjectProperty property;
    // Start is called before the first frame update
    void Start()
    {
        vcam = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera as CinemachineVirtualCamera;
        fx_settings = vcam.GetComponent<CinemachineVolumeSettings>();
        foreach(var fx in fx_settings.m_Profile.components)
        {
            if (fx is UnityEngine.Rendering.Universal.Vignette)
            {
                vignette = fx as UnityEngine.Rendering.Universal.Vignette;
                break;
            }
        }
        vignette.active=true;
        vignette.intensity.value=0;  
    }

    public void SetOwner(Character character)
    {
        owner = character;
        property = owner.PropertyManager.GetPropertyByName("Health");
        property.RegisterChangeCallback(OnValueChanged);
        if (vignette!=null)
            vignette.intensity.value=0;
    }

    void OnValueChanged(float oldCurValue, float newCurValue, float oldValue, float newValue)
    {
        vignette.intensity.value=0.5f-newCurValue/property.BaseValue/2;
    }

    private void OnEnable() {
        if (vignette!=null&&owner!=null)
        {
            vignette.active=true;
        }
    }

    private void OnDisable() {
        vignette.active=false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
