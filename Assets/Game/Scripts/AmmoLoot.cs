using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class AmmoLoot : MonoBehaviour
{
    [Tooltip("Снаряды, добавляемые игроку при подборе предмета")]
    [SerializeField]
    protected Ammunition[] ammoToAdd;
    [SerializeField]
    protected MMFeedbacks onAddFeedback;

    protected bool isUsed = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isUsed) return;
        var inventory = other.GetComponent<InventoryHandler>();
        if (inventory != null)
        {
            for (int i = 0; i < ammoToAdd.Length; i++)
            {
                inventory.AddAmmunition(ammoToAdd[i].name, ammoToAdd[i].count);   
            }
            var character = other.GetComponent<Character>();
            character.onReload?.Invoke();

            isUsed = true;
            onAddFeedback.PlayFeedbacks();
        }
    }
}
