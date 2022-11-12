using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using Random = UnityEngine.Random;


public class CircleRandomSpawner : MonoBehaviour
{
    [Tooltip("Центр круга в котором будет происходить спавн")]
    [SerializeField]
    protected Transform spawnCenter;
    [Tooltip("Радиус круга в котором происходит спавн")]
    [SerializeField]
    protected float radius;

    public GameObject Spawn(GameObject objectToSpawn)
    {
        float x = Random.Range(0, radius);
        float y = Mathf.Sqrt(radius * radius - x * x);

        return Instantiate(objectToSpawn,spawnCenter.position+new Vector3(x,y,0),Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        MMDebug.DrawPoint(spawnCenter.position,Color.green, radius);
    }
}
