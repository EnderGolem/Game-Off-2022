using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

public class GameEventTrigger : MonoBehaviour
{
    [SerializeField]
    string eventName;
     
    public bool TriggerOnStart;
    void Start()
    {
        if (TriggerOnStart)
        {
            Trigger();
        }
    }

    public void Trigger()
    {
        MMGameEvent.Trigger(eventName);
    }
}
