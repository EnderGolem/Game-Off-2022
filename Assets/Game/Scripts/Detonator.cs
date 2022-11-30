using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detonator : MonoBehaviour
{
    PropertyManager _propertyManager;
    ObjectProperty health;
    void Start()
    {
        _propertyManager = GetComponent<PropertyManager>();
        ObjectProperty health = _propertyManager.GetPropertyByName("Health");
    }

    public void Kill()
    {
        health.ChangeCurValue(-9999);
    }
}
