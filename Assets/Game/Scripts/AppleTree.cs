using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Health))]
public class AppleTree : MonoBehaviour
{ 
    [Tooltip("Суммарный урон, который необходимо нанести, чтобы сбить i-ое яблоко")]
    [SerializeField]
    protected float[] damageToThrowApples;
    [Tooltip("Используем ли мы случайный взвешенный выбор яблок или же используем четко заданную" +
             "последовательность")]
    [SerializeField]
    protected bool useRandomSequence;
    [Tooltip("последовательность яблок если мы ее используем")]
    [SerializeField]
    protected GameObject[] appleSequence;
    [Tooltip("Взвешенный список яблок, которые могут упасть")]
    [SerializeField]
    protected AppleWithWeight[] apples;

    protected Health _health;

    protected CircleRandomSpawner spawner;
    /// <summary>
    /// суммарный урон, полученный деревом
    /// </summary>
    protected float totalDamage = 0;
    /// <summary>
    /// количество яблок, которые уже упали
    /// </summary>
    protected int thrownApples;
    private void Awake()
    {
        _health = GetComponent<Health>();
        spawner = GetComponent<CircleRandomSpawner>();
    }
    

    public void OnDamage(float damage)
    {   
        ///Если мы уже сбросили все яблоки, то делать нам болше нечего
        if(thrownApples >= damageToThrowApples.Length) return;
        if (damage > 0)
        {
            totalDamage += damage;
        }

        int newThrow = 0;

        for (int i = thrownApples; i < damageToThrowApples.Length; i++)
        {
            if (totalDamage >= damageToThrowApples[i])
            {
                newThrow++;
            }
            else
            {
                break;
            }
        }

        for (int i = 0; i < newThrow; i++)
        {
            ThrowApple();
        }
    }

    protected void ThrowApple()
    {
        if (useRandomSequence)
        {
            float sum = 0;
            for (int i = 0; i < apples.Length; i++)
            {
                sum += apples[i].weight;
            }

            float rand = Random.Range(0, sum);

            sum = 0;
            float lastSum = 0;
            for (int i = 0; i < apples.Length; i++)
            {
                sum += apples[i].weight;
                if (rand >= lastSum && rand < sum)
                {
                    spawner.Spawn(apples[i].apple);
                    break;
                }
            }
        }
        else
        {
            spawner.Spawn(appleSequence[thrownApples]);
        }

        thrownApples++;
    }

    private void OnEnable()
    {
        if (_health != null)
        {
            _health.OnDamage += OnDamage;
        }
    }

    private void OnDisable()
    {
        if (_health != null)
        {
            _health.OnDamage -= OnDamage;
        }
    }

}
[Serializable]
public class AppleWithWeight
{
    public  GameObject apple;
    public float weight;
}
