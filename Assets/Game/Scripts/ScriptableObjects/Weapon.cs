using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon", order = 11)]
public class Weapon : ScriptableObject
{
   [SerializeField]
   protected string itemName;
   [Tooltip("Способности персонажа, активируемые при выборе этого оружия")]
   [SerializeField]
   protected string[] activeAbilities;
   [Tooltip("Максимальное накопление использований оружия")]
   [SerializeField]
   protected int maxAmmoCount;
   [Tooltip("Тип заряжаемых боеприпасов")]
   [SerializeField]
   protected string ammunitionType;

   public string ItemName => itemName;
   public int MaxAmmoCount => maxAmmoCount;

   public string AmmunitionType => ammunitionType;
   public string[] ActiveAbilities => activeAbilities;
}
