using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ShakingCamera : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    CinemachineBasicMultiChannelPerlin noise;
    public static ShakingCamera instance;

    float _amplitude;
    float _duration;
    float _fadeCoef;
    bool _shaking = false;

    void Start()
    {
        instance=this;
        noise = vcam.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
        noise.m_FrequencyGain=0;
    }
    public void Shake(float amplitude = 1, float duration = 1)
    {
        _amplitude=amplitude;
        _duration=duration;
        _fadeCoef=_amplitude/_duration;
        _shaking=true;
        noise.m_AmplitudeGain=amplitude;
        noise.m_FrequencyGain=1;
    }
    private void Update() {
        if (_shaking)
        {
            _duration-=Time.deltaTime;
            noise.m_AmplitudeGain-=_fadeCoef*Time.deltaTime;
            if (_duration<=0)
            {
                _duration=0;
                _shaking=false;
                noise.m_FrequencyGain=0;
            }
        }
    }
}
