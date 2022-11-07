using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon", order = 11)]
public class Weapon : ScriptableObject
{
   [Tooltip("Способности персонажа, активируемые при выборе этого оружия")]
   [SerializeField]
   protected string[] activeAbilities;

   public string[] ActiveAbilities => activeAbilities;
}
