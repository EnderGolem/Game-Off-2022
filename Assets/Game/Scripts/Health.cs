using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Tooltip("Максимальное здоровье персонажа")]
    [SerializeField]
    protected float maxHealth;
    [Tooltip("Количество здоровья с которым персонаж появляется")]
    [SerializeField]
    protected float initialHealth;
    [Tooltip("Должен ли объект быть уничтожен при смерти")]
    [SerializeField]
    protected bool destroyOnDeath;
    [Tooltip("Время после смерти перед уничтожением объекта")]
    [SerializeField]
    protected float timeBeforeDestruction;
    
    protected PropertyManager _propertyManager;
    protected ObjectProperty hProperty;

    protected Character owner;
    void Start()
    {
        owner = gameObject.GetComponentInParent<Character>();
        _propertyManager = GetComponent<PropertyManager>();
        if (_propertyManager == null)//гарантия того что объекты с компонентом health имеют PropertyManager
        {
            _propertyManager = gameObject.AddComponent<PropertyManager>();
        }
        
        hProperty=_propertyManager.AddProperty("Health", maxHealth, initialHealth);
        hProperty.RegisterChangeCallback(OnHealthChanged);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void CheckDeath()
    {
        if (hProperty.GetCurValue() <= 0)
        {
            Kill();
        }
    }

    protected void Kill()
    {
        if (owner != null)
        {
            owner.Kill();
        }

        if (destroyOnDeath)
        {
            Invoke(nameof(DestroySelf), timeBeforeDestruction);
        }
    }

    protected void DestroySelf()
    {
        Destroy(gameObject);
    }

    protected void OnHealthChanged(float oldCurValue, float newCurValue, float oldValue, float newValue)
    {
        CheckDeath();
    }
}
