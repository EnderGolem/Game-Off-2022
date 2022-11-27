using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Explosion : MonoBehaviour
{
    public float intensity;
    public float power;
    public float duration;
    bool _lightEnd;
    bool _lightBegin;
    float _lightBeginCoef;
    float _lightEndCoef;
    public Light2D _light;
    public void Explode() {
        //_light=GetComponent<Light2D>();
        transform.SetParent(null);
        _lightBegin=true;
        ShakingCamera.instance?.Shake(power,duration);
        _lightBeginCoef=intensity/duration*2;
        _lightEndCoef=(intensity/duration);
    }
    private void Update() {
        if (_lightBegin)
        {
            _light.intensity+=_lightBeginCoef*Time.deltaTime;
            if (_light.intensity>=100)
            {
                _light.intensity=100;
                _lightBegin=false;
                _lightEnd=true;
            }
        }
        if (_lightEnd)
        {
            _light.intensity-=_lightEndCoef*Time.deltaTime;
            if (_light.intensity<=0)
            {
                Destroy(gameObject);
            }
        }
    }
}
