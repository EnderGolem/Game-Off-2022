using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoltsCounter : MonoBehaviour
{
    InventoryHandler inventory;
    public TextMeshProUGUI count;
    public string item_name;
    Character owner;

    public void SetOwner(Character character)
    {
        if (character == null) return;
        owner = character;
        owner.TryGetComponent<InventoryHandler>(out inventory);
        owner.onReload.AddListener(OnValueChanged);
        count.text = 'x'+inventory.GetCount(item_name).ToString();
    }

    public void OnValueChanged()
    {
        count.text = 'x'+inventory.GetCount(item_name).ToString();
    }
}
