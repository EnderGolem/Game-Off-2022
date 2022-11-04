using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Effect", menuName = "Effect", order = 1)]
public class EffectDescription : ScriptableObject
{
   [SerializeField] private int id;
   [Tooltip("Задает время действия эффекта в секундах")]
   [SerializeField] private float duration;
   [Tooltip("Шанс срабатывания эффекта")]
   [Range(0,1)]
   [SerializeField] private float chance;
   
   [SerializeField] private int maxStackCount;
   [Tooltip("Стихия эффекта")]
   [SerializeField] private EffectNature nature;
   [Tooltip("Список воздействий эффекта")]
   [SerializeField] private ImpactDescription[] impacts;

   public int Id => id;

   public float Duration => duration;

   public float Chance => chance;

   public int MaxStackCount => maxStackCount;

   public EffectNature Nature => nature;

   public ImpactDescription[] Impacts => impacts;

}
[Serializable]
public class ImpactDescription
{
    [Tooltip("Идентификатор свойства на которое направлено воздействие")]
    [SerializeField] private int targetId;
    [Tooltip("Тип воздействия")]
    [SerializeField] private ImpactType type;
    [Tooltip("Значение воздействия в случае если свойство его скалирования не найдено")]
    [SerializeField] private float defaultValue;
    [Tooltip("Идентификатор свойства, определяющего значение воздействия")]
    [SerializeField] private int propertyId;

    public int TargetId => targetId;

    public ImpactType Type => type;

    public float DefaultValue => defaultValue;

    public int PropertyId => propertyId;
}
